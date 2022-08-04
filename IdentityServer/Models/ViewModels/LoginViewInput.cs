using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models.ViewModels
{
    public class LoginViewInput
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
