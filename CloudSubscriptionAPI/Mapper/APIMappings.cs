using AutoMapper;
using Entities;
using Entities.DTOs;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace CloudSubscriptionAPI.Mapper
{
    public class APIMappings : Profile
    {
        public APIMappings()
        {
            CreateMap<CloudServices, CloudServicesDTO>().ReverseMap();
            CreateMap<UserDTO, Users>().ReverseMap();
        }

    }
}
