using Entities;

namespace CloudSubscriptionAPI.Repository.IRepository
{
    public interface ICloudServiceRepository
    {
        Task<CloudServices> GetCloudServiceByid(int Id);
        Task<IEnumerable<CloudServices>> GetAllCloudServices();
    }
}
