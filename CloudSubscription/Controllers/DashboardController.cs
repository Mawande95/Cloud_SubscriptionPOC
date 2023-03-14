using CloudSubscription.Controllers;
using Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace CloudSubscriptionWeb.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public DashboardController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var id = identity.Claims.FirstOrDefault(c => c.Type == "ServiceId").Value;
            CloudServices obj = null;
            int Id = int.Parse(id);
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, SD.CloudServicesAPIPath + "GetCloudServiceById/" + Id);
                var client = _clientFactory.CreateClient();
                HttpResponseMessage apiResponse = await client.SendAsync(request);
                var jsonString = await apiResponse.Content.ReadAsStringAsync();
                obj = JsonConvert.DeserializeObject<CloudServices>(jsonString);
                return View(obj);
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }
    }
}
