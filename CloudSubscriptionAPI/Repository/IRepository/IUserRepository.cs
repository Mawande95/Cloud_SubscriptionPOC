using Entities;

namespace CloudSubscriptionAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<bool> Create(Users user);
        Task<Users> CheckEmailAddressExist(string emailAddress);
        Task<Users> Authenticate(string emailAddress, string password);
        Task<Users> GetUserById(Guid Id);
    }
}
