using CloudSubscription.Controllers;
using CloudSubscription.Models;
using Entities;
using Entities.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace CloudSubscriptionWeb.Controllers
{
    public class AccountController : Controller
    {
        const string serviceId = "_Id";
        private readonly IHttpClientFactory _clientFactory;

        public AccountController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<IActionResult> Login(int? Id)
        {
            if(User.Identity.IsAuthenticated)
            {
                HttpContext.SignOutAsync();
                return RedirectToAction("Login");
            }
            if (Id > 0)
            {
                HttpContext.Session.SetString(serviceId, Id.ToString());
            }
            
            return View();
        }
        public async Task<IActionResult> Register()
        {

            return View();
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public async Task<JsonResult> Register(UserDTO user)
        {

            try
            {

                ////////////////////////////////////////////////////
                var request = new HttpRequestMessage(HttpMethod.Post, SD.UserAPIPath + "CreateUser/");

                request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                var client = _clientFactory.CreateClient();
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return Json(new { valid = true, message = "You have succesfully register to the system", value = 1 });
                }

                return Json(new { valid = false, message = "Something went wrong when saving, please try again" });
            }
            catch (Exception)
            {

                throw;
            }


        }
        [HttpPost]
        public async Task<JsonResult> CheckEmailAddressExist(string EmailAddress)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, SD.UserAPIPath + "EmailAddressExistByEmailAddress/" + EmailAddress);

            var client = _clientFactory.CreateClient();
            HttpResponseMessage apiResponse = await client.SendAsync(request);

            if (apiResponse.StatusCode == HttpStatusCode.OK)
            {
                var jsonString = await apiResponse.Content.ReadAsStringAsync();

                Entities.Validation results = JsonConvert.DeserializeObject<Entities.Validation>(jsonString);

                if (results.Error == true)
                {
                    return Json(new { results.Error, message = results.Message });
                }
            }

            return Json(new { message = "" });
        }
        [HttpPost]
        public async Task<JsonResult> Login(Login user)
        {

            try
            {
                bool valid = false;
                if (ModelState.IsValid)
                {
                    ////////////////////////////////////////////////////
                    ///

                    var request = new HttpRequestMessage(HttpMethod.Post, SD.UserAPIPath + "LoginUser/");
                    if (user != null)
                    {
                        request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                    }
                    else
                    {
                        new Users();
                    }

                    var client = _clientFactory.CreateClient();
                    HttpResponseMessage response = await client.SendAsync(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {

                        var jsonString = await response.Content.ReadAsStringAsync();

                        Users results = JsonConvert.DeserializeObject<Users>(jsonString);

                        if (results.Token == null)
                        {
                            return Json(new { valid, message = "" });
                        }
                        else
                        {
                            await Login1(results.Token, results);
                            return Json(new { valid = true, message = "" });


                        }
                    }
                    return Json(new { valid = false, message = "Invalid Username or Password" });
                }
                return Json(new { valid = false, message = "Invalid Username or Password" });
            }
            catch (Exception)
            {

                throw;
            }


        }
        public async Task<Boolean> Login1(string token, Users user)
        {
            try
            {
                if (string.IsNullOrEmpty(token)) return false;

                var validationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("THIS IS USED TO SIGN AND VERIFY JWT TOKENS, REPLACE IT WITH YOUR OWN SECRET, IT CAN BE ANY STRING")),
                    ValidAudience = "",
                    ValidateIssuer = false,
                    ValidateLifetime = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = false
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                //principal.Identity.Name
                var identity = principal.Identity as ClaimsIdentity;

                var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
                var extraClaims = securityToken.Claims.Where(c => !identity.Claims.Any(x => x.Type == c.Type)).ToList();

                extraClaims.Add(new Claim("jwt", token));
                identity.AddClaim(new Claim("Names", user.FirstName + " " + user.LastName));
                identity.AddClaim(new Claim("UserId", user.Id.ToString()));
                identity.AddClaim(new Claim("ServiceId", HttpContext.Session.GetString(serviceId)));
                var authenticationProperties = new AuthenticationProperties()
                {
                    IssuedUtc = DateTime.Now,
                    ExpiresUtc = DateTime.Now,
                    IsPersistent = false,

                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                

                return identity.IsAuthenticated;
            }
            catch (Exception ex)
            {

            }

            return false;
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.SetString("Token", "");
            return RedirectToAction("Login");
        }
    }
}
