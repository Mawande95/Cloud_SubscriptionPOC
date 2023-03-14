using Entities;
using Entities.DTOs;

namespace CloudSubscriptionAPI.Service.Iservice
{
    public interface IUserService
    {
        Task<bool> Create(UserDTO userDTO);
        Task<Users> CheckEmailAddressExist(string emailAddress);
        Task<Users> Authenticate(Login userLogin);
        Task<Users> GetUserById(Guid Id);
    }
}
