using AutoMapper;
using CloudSubscriptionAPI.Repository.IRepository;
using CloudSubscriptionAPI.Service.Iservice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CloudSubscriptionAPI.Controllers
{
    [Route("api/v{version:apiVersion}/CloudServices")]
    [ApiController]
    public class CloudServicesController : ControllerBase
    {
        private readonly ICloudService _service;
        public CloudServicesController(ICloudService service)
        {
            _service = service;
        }
        [HttpGet("GetAllCloudServices")]
        public async Task<ActionResult> GetAllCloudServices()
        {
            var objList = await _service.GetAllCloudServices();
            return Ok(objList);
        }

        [HttpGet("GetCloudServiceById/{Id}")]
        public async Task<ActionResult> GetCloudServiceById(string Id)
        {
            int id = int.Parse(Id);
            var objList = await _service.GetCloudServiceByid(id);

            return Ok(objList);
        }

    }
}
