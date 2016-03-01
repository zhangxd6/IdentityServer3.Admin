using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owin;
using IdentityAdmin.Configuration.Hosting;

namespace IdentityAdmin.Configuration
{
    public class ExternalAdminSecurtityConfiguration : AdminSecurityConfiguration
    {
        public string AuthorizationEndpoint { get; set; }

        public string Authority { get; set; }

        public IEnumerable<string> RequiredScopes { get; set; }

        internal override void Validate()
        {
            base.Validate();
            if (String.IsNullOrWhiteSpace(AuthorizationEndpoint)) throw new Exception("AuthorizationEndpoint is required.");
            if (String.IsNullOrWhiteSpace(Authority)) throw new Exception("Authority is required.");

        }
        public override void Configure(IAppBuilder app)
        {
            app.UseExternalIdentityServer(this);
        }
    }
}
