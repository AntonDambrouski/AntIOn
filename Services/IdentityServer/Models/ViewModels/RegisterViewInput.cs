using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models.ViewModels
{
    public class RegisterViewInput
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirm password must be the same")]
        public string ConfirmPassword { get; set; }
        public string ReturnUrl { get; set; }
    }
}
