using CloudSubscriptionWeb.Payments;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PayPal.Api;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Channels;

namespace CloudSubscriptionWeb.Controllers
{
    public class BillingController : Controller
    {
        private readonly AppSettings _appSettings;
        private PayPal.Api.Payment payment;
        private readonly IHttpClientFactory _clientFactory;
        public BillingController(IOptions<AppSettings> appSettings, IHttpClientFactory clientFactory)
        {
            _appSettings = appSettings.Value;
            _clientFactory = clientFactory;
        }
        public async Task<IActionResult> Index(string cancel = null, string blogId = "", string payerId = "", string guid = "")
        {
            if (User.Identity.IsAuthenticated)
            {

                var clientId = _appSettings.ClientId.ToString();
                var secretKey = _appSettings.SecretPayPal.ToString();
                var mode = _appSettings.Mode.ToString();
                APIContext apiContext = PaypalConfiguration.GetAPIContext(clientId, secretKey, mode);
                try
                {
                    string payerID = payerId;
                    if (string.IsNullOrEmpty(payerID))
                    {
                        string baseUrl = this.Request.Scheme + "://" + Request.Host + "/Billing/Index?";
                        guid = Guid.NewGuid().ToString();
                        var createdPayment = await this.CreatePayment(apiContext, baseUrl + "guid=" + guid, blogId);
                        var links = createdPayment.links.GetEnumerator();
                        string paypalRedirectUrl = null;
                        while (links.MoveNext())
                        {
                            Links link = links.Current;
                            if (link.rel.ToLower().Trim().Equals("approval_url"))
                            {
                                paypalRedirectUrl = link.href;
                            }
                        }
                        HttpContext.Session.SetString("Payment", createdPayment.id);
                        return Redirect(paypalRedirectUrl);

                    }
                    else
                    {
                        var paymentId = HttpContext.Session.GetString("Payment");
                        var executedPayment = ExecutePayment(apiContext, payerId, paymentId);
                        if (executedPayment.state.ToLower() != "approved")
                        {
                            return View("PaymentFailded");

                        }
                        var blogsIds = executedPayment.transactions[0].item_list.items[0].sku;
                        return View("PaymentSuccess");
                    }
                }
                catch (Exception)
                {

                    return View("PaymentFailded");
                }

            }
            return View();
        }
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId,
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);

        }
        private async Task<Payment> CreatePayment(APIContext apiContext, string redirectUrl, string blogId)
        {

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var id = identity.Claims.FirstOrDefault(c => c.Type == "ServiceId").Value;

            var itemList = new ItemList()
            {
                items = new List<Item>() { }
            };
            CloudServices obj = null;
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, SD.CloudServicesAPIPath + "GetCloudServiceById/" + id);
                var client = _clientFactory.CreateClient();
                HttpResponseMessage apiResponse = await client.SendAsync(request);
                var jsonString = await apiResponse.Content.ReadAsStringAsync();
                obj = JsonConvert.DeserializeObject<CloudServices>(jsonString);
                var usdPerRand = decimal.Parse(_appSettings.UsdPerRand, new NumberFormatInfo() { NumberDecimalSeparator = "," });
                
                decimal usd = Math.Round((Decimal)(obj.Price / usdPerRand), 2);
                itemList.items.Add(new Item()
                {
                    name = obj.ServiceName,
                    currency = "USD",
                    price = usd.ToString(),
                    quantity = "1",
                    sku = "asd"
                });
                var payer = new Payer()
                {
                    payment_method = "paypal"
                };
                var redirUrls = new RedirectUrls()
                {
                    cancel_url = redirectUrl + "&Cancel=true",
                    return_url = redirectUrl
                };
                var amount = new Amount()
                {
                    currency = "USD",
                    total = usd.ToString()
                };
                var transaction = new List<Transaction>();
                transaction.Add(new Transaction()
                {
                    description = "Transcation Description",
                    invoice_number = Guid.NewGuid().ToString(),
                    amount = amount,
                    item_list = itemList,
                });
                this.payment = new Payment()
                {
                    intent = "Sale",
                    payer = payer,
                    transactions = transaction,
                    redirect_urls = redirUrls
                };
                return this.payment.Create(apiContext);

            }
            catch (Exception)
            {
                throw;
            }

        }
        public IActionResult Cancel()
        {
            return View();
        }
        public IActionResult PaymentFailded()
        {
            return View();
        }
        public IActionResult PaymentSuccess()
        {
            return View();
        }
    }
}
