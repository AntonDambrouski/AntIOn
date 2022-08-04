namespace IdentityServer.Models.ViewModels
{
    public class LoginViewModel : LoginViewInput
    {
        public IEnumerable<ViewError>? Errors { get; set; }
    }
}
