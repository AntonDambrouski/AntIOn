namespace IdentityServer.Models.ViewModels
{
    public class LoginViewInput
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
