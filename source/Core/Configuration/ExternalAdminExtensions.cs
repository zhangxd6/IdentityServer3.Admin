using Owin;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAdmin.Configuration
{
    public static class ExternalAdminExtensions
    {

        public static void UseExternalIdentityServer(this IAppBuilder app, ExternalAdminSecurtityConfiguration config)
        {
            JwtSecurityTokenHandler.InboundClaimTypeMap.Clear();
            var options = new IdentityServer3.AccessTokenValidation.IdentityServerBearerTokenAuthenticationOptions()
            {
                Authority = config.Authority,
            };
            if (config.RequiredScopes!=null)
            {
                options.RequiredScopes = config.RequiredScopes;
            }
            app.UseIdentityServerBearerTokenAuthentication(options);

        }
    }
}
