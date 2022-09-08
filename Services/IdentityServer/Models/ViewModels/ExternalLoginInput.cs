using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models.ViewModels
{
    public class ExternalLoginInput
    {
        [Required]
        public string Name { get; set; }
        public string ReturnUrl { get; set; }
    }
}
