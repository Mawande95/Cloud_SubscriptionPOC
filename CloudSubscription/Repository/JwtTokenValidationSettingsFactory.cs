using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CloudSubscriptionWeb.Repository.IRepository;

namespace CloudSubscriptionWeb.Repository
{
    public class JwtTokenValidationSettingsFactory : IJwtTokenValidationSettings
    {
        private readonly JwtTokenValidationSettings settings;

        public String ValidIssuer => settings.ValidIssuer;
        public Boolean ValidateIssuer => settings.ValidateIssuer;
        public String ValidAudience => settings.ValidAudience;
        public Boolean ValidateAudience => settings.ValidateAudience;
        public String SecretKey => settings.SecretKey;

        public JwtTokenValidationSettingsFactory(IOptions<JwtTokenValidationSettings> options)
        {
            settings = options.Value;
        }

        public TokenValidationParameters CreateTokenValidationParameters()
        {
            var result = new TokenValidationParameters
            {
                ValidateIssuer = ValidateIssuer,
                ValidIssuer = ValidIssuer,

                ValidateAudience = ValidateAudience,
                ValidAudience = ValidAudience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey)),

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };

            return result;
        }
    }
}
