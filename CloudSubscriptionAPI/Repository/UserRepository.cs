using CloudSubscriptionAPI.Data;
using CloudSubscriptionAPI.Repository.IRepository;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Core;


namespace CloudSubscriptionAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _repo;
        private readonly ILogger<Users> _logger;
        public UserRepository(ApplicationDbContext repo, ILogger<Users> logger)
        {
            _repo = repo;
            _logger = logger;
        }
        public async Task<Users> GetUserById(Guid Id)
        {
            try
            {
                var user = await _repo.Users.FindAsync(Id);
                return user;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> Create(Users user)
        {
            bool results = false;
            try
            {

                await _repo.Users.AddAsync(user);
                await Save();
                results = true;

            }
            catch (Exception)
            {

                throw;
            }
            return results;
        }
        public async Task<Users> CheckEmailAddressExist(string emailAddress)
        {
            bool results = false;
            try
            {
                var data = await _repo.Users.AsNoTracking().Where(u => u.EmailAddress == emailAddress).SingleOrDefaultAsync(); ;
                return data;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<Users> Authenticate(string emailAddress, string password)
        {
            if (string.IsNullOrEmpty(emailAddress) || string.IsNullOrEmpty(password))
            {
                return null;
            }
            try
            {

                var user = await _repo.Users.AsNoTracking().SingleOrDefaultAsync(u => u.EmailAddress == emailAddress);

                if (user == null)
                {

                    return null;
                }

                if (!CloudSubscriptionAPI.Generics.Generics.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                {
                    return null;
                }

                return user;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public async Task<bool> Save()
        {
            bool successfull = false;
            try
            {
                await _repo.SaveChangesAsync();

                successfull = true;
            }
            catch (OptimisticConcurrencyException exc)
            {
                //TODO: Error handling;
            }
            catch (UpdateException exc)
            {
                //TODO: Error handling;
            }
            catch (Exception exc)
            {
                //TODO: Error handling;
            }

            return successfull;
        }
    }
}
