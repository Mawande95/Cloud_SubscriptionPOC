using AutoMapper;
using CloudSubscriptionAPI.Data;
using CloudSubscriptionAPI.Repository.IRepository;
using CloudSubscriptionAPI.Service.Iservice;
using Entities;
using Entities.DTOs;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.Data.Entity.Core;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace CloudSubscriptionAPI.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        public UserService(IUserRepository repo, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _repo = repo;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        public Task<Users> GetUserById(Guid Id)
        {
            try
            {
                var user = _repo.GetUserById(Id);
                return user;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> Create(UserDTO userDTO)
        {
            bool results = false;
            try
            {
                var UserObj = _mapper.Map<Users>(userDTO);
                byte[] passwordHash, passwordSalt;
                Generics.Generics.CreatePasswordHash(userDTO.Password, out passwordHash, out passwordSalt);
                UserObj.PasswordHash = passwordHash;
                UserObj.PasswordSalt = passwordSalt;
                UserObj.Id = Generics.Generics.GenerateGuid();
                var resulst = await _repo.Create(UserObj);
                results = true;

            }
            catch (Exception)
            {

                throw;
            }
            return results;
        }
        public Task<Users> CheckEmailAddressExist(string emailAddress)
        {
            try
            {
                var data = _repo.CheckEmailAddressExist(emailAddress);
                return data;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<Users> Authenticate(Login userLogin)
        {

            var user = await _repo.Authenticate(userLogin.EmailAddress.Trim(), userLogin.Password);
            if (user != null)
            {
                var claims = new List<Claim>();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Issuer = "Mawande",
                    Audience = ""

                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenString = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
                user.Token = tokenString;
            }
            return user;

        }

    }
}
