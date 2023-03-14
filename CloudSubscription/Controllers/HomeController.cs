using CloudSubscription.Models;
using CloudSubscriptionWeb;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace CloudSubscription.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            
            IEnumerable<CloudServices> obj = null;
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, SD.CloudServicesAPIPath + "GetAllCloudServices");
                var client = _clientFactory.CreateClient();
                HttpResponseMessage apiResponse = await client.SendAsync(request);
                var jsonString = await apiResponse.Content.ReadAsStringAsync();
                obj = JsonConvert.DeserializeObject<IEnumerable<CloudServices>>(jsonString);
                return View(obj);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}