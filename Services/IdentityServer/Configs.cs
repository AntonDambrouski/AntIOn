using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace IdentityServer
{
    public class Configs
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<Client> GetClients() =>
            new Client[]
            {
                new Client
                {
                    ClientId = "angular",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "https://localhost:5003/signin-oidc" },

                    AllowedScopes = 
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile 
                    }
                }
            };
    }
}
