using AutoMapper;
using CloudSubscriptionAPI.Repository.IRepository;
using CloudSubscriptionAPI.Service.Iservice;
using Entities;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CloudSubscriptionAPI.Controllers
{
    [Route("api/v{version:apiVersion}/Users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("CreateUser")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO userDto)
        {
            if (userDto != null)
            {
                var user = await _userService.Create(userDto);
                if (user)
                {
                    return Ok(StatusCodes.Status200OK);
                }
                else
                {
                    return Ok(StatusCodes.Status400BadRequest);
                }
            }
            return Ok(StatusCodes.Status400BadRequest);
        }
        [HttpGet("CheckEmailAddressExist/{emailAddress}")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckEmailAddressExist(string emailAddress)
        {
            if (emailAddress != null)
            {
                using (var validation = new Entities.Validation())
                {
                    var user = await _userService.CheckEmailAddressExist(emailAddress);
                    if (user != null)
                    {
                        validation.Error = true;
                        validation.Message = "Email address " + emailAddress + " exist";
                    }

                    return Ok(validation);
                }
            }
            return Ok(StatusCodes.Status400BadRequest);
        }
        [HttpGet("GetUserById/{Id}")]
        public async Task<ActionResult> GetUserById(Guid Id)
        {
            var obj = await _userService.GetUserById(Id);
            return Ok(obj);
        }
        [HttpPost("LoginUser")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser([FromBody] Login userLogin)
        {
            var user = await _userService.Authenticate(userLogin);

            if (userLogin == null || user == null)
            {

                return BadRequest("Invalid Username or Password");
            }
            return Ok(user);

        }
    }
}
