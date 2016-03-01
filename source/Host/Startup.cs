﻿/*
 * Copyright 2014 Dominick Baier, Brock Allen, Bert Hoorne
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections.Generic;
using System.Web.Security;
using Owin;
using Microsoft.Owin;
using IdentityAdmin.Configuration;
using IdentityAdmin.Core;
using IdentityAdmin.Host;
using IdentityAdmin.Host.InMemoryService;
using IdentityAdmin.Logging;

[assembly: OwinStartup(typeof (StartupWithLocalhostSecurity))]

namespace IdentityAdmin.Host
{
    public class StartupWithLocalhostSecurity
    {
        public void Configuration(IAppBuilder app)
        {
            LogProvider.SetCurrentLogProvider(new TraceSourceLogProvider());

            app.Map("/admin", adminApp =>
            {
                var factory = new IdentityAdminServiceFactory
                {
                    IdentityAdminService = new Registration<IIdentityAdminService, InMemoryIdentityManagerService>()
                };
                var rand = new System.Random();
                var clients = ClientSeeder.Get(rand.Next(1000, 3000));
                var scopes = ScopeSeeder.Get(rand.Next(15));
                factory.Register(new Registration<ICollection<InMemoryScope>>(scopes));
                factory.Register(new Registration<ICollection<InMemoryClient>>(clients));
                adminApp.UseIdentityAdmin(new IdentityAdminOptions
                {
                    Factory = factory,
                    AdminSecurityConfiguration = new ExternalAdminSecurtityConfiguration()
                    {
                        //RequireSsl = false,
                        AdminRoleName = "SSI\\Maroon Team",
                        BearerAuthenticationType = Constants.BearerAuthenticationType,
                        NameClaimType = "name",
                        RoleClaimType = "role",
                        ShowLoginButton = true,
                        AuthorizationEndpoint = "http://ssiplatform:34476/identity/connect/authorize",
                        Authority = "http://ssiplatform:34476/identity/"
                    }
                });
            });
        }
    }
}