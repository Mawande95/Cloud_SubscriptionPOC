using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudSubscriptionWeb.Repository
{
    public class JwtTokenValidationSettings
    {
        public String ValidIssuer { get; set; }
        public Boolean ValidateIssuer { get; set; }

        public String ValidAudience { get; set; }
        public Boolean ValidateAudience { get; set; }

        public String SecretKey { get; set; }

        public static TokenValidationParameters CreateTokenValidationParameters()
        {
            var result = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("tetet")),

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };

            return result;
        }
    }
}
