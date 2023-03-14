using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudSubscriptionWeb.Repository.IRepository
{
    public interface IJwtTokenValidationSettings
    {
        String ValidIssuer { get; }
        Boolean ValidateIssuer { get; }

        String ValidAudience { get; }
        Boolean ValidateAudience { get; }

        String SecretKey { get; }

        TokenValidationParameters CreateTokenValidationParameters();
    }
}
