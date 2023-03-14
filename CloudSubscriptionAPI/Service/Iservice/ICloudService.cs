using Entities;

namespace CloudSubscriptionAPI.Service.Iservice
{
    public interface ICloudService
    {
        Task<CloudServices> GetCloudServiceByid(int Id);
        //Task<bool> CreatePayment3(Guid userid, int serviceId);
        Task<IEnumerable<CloudServices>> GetAllCloudServices();

    }
}
