using CloudSubscriptionAPI.Data;
using CloudSubscriptionAPI.Repository.IRepository;
using Entities;
using Microsoft.EntityFrameworkCore;


namespace CloudSubscriptionAPI.Repository
{
    public class CloudServiceRepository : ICloudServiceRepository
    {
        private readonly ApplicationDbContext _repo;

        public CloudServiceRepository(ApplicationDbContext repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<CloudServices>> GetAllCloudServices()
        {
            try
            {
                return await _repo.CloudServices.AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<CloudServices> GetCloudServiceByid(int id)
        {
            try
            {
                return await _repo.CloudServices.AsNoTracking().Where(c => c.Id == id).SingleOrDefaultAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
