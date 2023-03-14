using AutoMapper;
using Azure.Core;
using CloudSubscriptionAPI.Data;
using CloudSubscriptionAPI.Repository.IRepository;
using CloudSubscriptionAPI.Service.Iservice;
using Entities;
using Microsoft.Extensions.Options;

namespace CloudSubscriptionAPI.Service
{
    public class CloudService : ICloudService
    {
        private readonly ICloudServiceRepository _repo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public CloudService(ICloudServiceRepository repo, IMapper mapper, IUserRepository userRepo, IOptions<AppSettings> appSettings)
        {
            _repo = repo;
            _mapper = mapper;
            _userRepo = userRepo;
            _appSettings = appSettings.Value;
        }
        public Task<IEnumerable<CloudServices>> GetAllCloudServices()
        {
            try
            {
                return _repo.GetAllCloudServices();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Task<CloudServices> GetCloudServiceByid(int id)
        {
            try
            {
                return _repo.GetCloudServiceByid(id);
            }
            catch (Exception)
            {

                throw;
            }
        }
        //public async Task<bool> CreatePayment3(Guid userid, int serviceId)
        //{
        //    throw new NotImplementedException();
        //    //bool results = false;
        //    //try
        //    //{
        //    //    var user = await _userRepo.GetUserById(userid);
        //    //    if (user == null)
        //    //    {
        //    //        var service = await _repo.GetCloudServiceByid(serviceId);
        //    //        if (service == null)
        //    //        {
        //    //            Payments payments = new Payments()
        //    //            {
        //    //                Id = service.Id,
        //    //                CustomerName = user.FirstName + " " + user.LastName,
        //    //                ContactNumber = user.ContactNumber,
        //    //                ServiceName = service.ServiceName,
        //    //                Price = service.Price
        //    //            };
        //    //        }
        //    //        results = false;
        //    //    }
        //    //    results = false;
        //    //}
        //    //catch (Exception)
        //    //{

        //    //    throw;
        //    //}
        //    //return results;

        //}
        //private void PaymentsWithPaypal(string cancel = null, string blogId = "", string payerId = "", string guid = "")
        //{
        //    var clientId = _appSettings.ClientId.ToString();
        //    var secretKey = _appSettings.SecretKey.ToString();
        //    var UrlAPI = _appSettings.UrlAPI.ToString();
        //    APIContext apiContext = PaypalConfiguration.GetAPIContext(clientId, secretKey, UrlAPI);
        //    try
        //    {
        //        string payerID = payerId;
        //        if (string.IsNullOrEmpty(payerID))
        //        {
        //            string baseUrl = _appSettings.ReturnApplicationUrl;
        //            var guidd = Generics.Generics.GenerateGuid();
        //            var createdPayment = this.CreatePayment(apiContext, baseUrl + "guid=" + guidd, blogId);
        //            var links = createdPayment.links.GetEnumerator();
        //            string paypalRedirectUrl = null;
        //            while (links.MoveNext()) { 
        //                var link = links.Current;
        //                if (link.rel.ToLower().Trim().Equals("approval_url"))
        //                {
        //                    paypalRedirectUrl = link.rel;
        //                }
        //            }
                    


        //        }
        //        else
        //        {

        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}
        //private Payment CreatePayment(APIContext apiContext, string redirectUrl, string blogId)
        //{
        //    var itemList = new ItemList()
        //    {
        //        items = new List<Item>() { }
        //    };
        //    itemList.items.Add(new Item()
        //    {
        //        name = "Item Detail",
        //        currency = "Rand",
        //        price = "2.00"
        //    });
        //    var payer = new Payer()
        //    {
        //        payment_method = "paypal"
        //    };
        //    var redirUrls = new RedirectUrls()
        //    {
        //        cancel_url = redirectUrl + "&Cancel=true",
        //        return_url= redirectUrl
        //    };

        //    var amount = new Amount()
        //    {
        //        currency = "USD",
        //        total = "2.00"
        //    };
        //    var transaction = new List<Transaction>();
        //    transaction.Add(new Transaction()
        //    {
        //        description = "Transcation Description",
        //        invoice_number = Generics.Generics.GenerateGuid().ToString(),
        //        amount = amount,
        //        item_list = itemList,
        //    });
        //    this.payment = new Payment()
        //    {
        //        intent = "Sale",
        //        payer = payer,
        //        transactions = transaction,
        //        redirect_urls = redirUrls
        //    };
        //    return this.payment.Create(apiContext);

        //}
    }
}

