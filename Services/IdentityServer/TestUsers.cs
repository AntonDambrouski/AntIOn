using Duende.IdentityServer.Test;
using IdentityModel;
using System.Security.Claims;

namespace IdentityServer
{
    public class TestUsers
    {
        public static List<TestUser> Users =>
            new()
            {
                new TestUser
                {
                    SubjectId = "12",
                    Username = "John",
                    Password = "Passw123$",
                    Claims = new Claim[]
                    {
                        new Claim(ClaimTypes.DateOfBirth, "10.12.2000"),
                        new Claim(JwtClaimTypes.WebSite, "http://john.com")
                    }
                }
            };
    }
}
