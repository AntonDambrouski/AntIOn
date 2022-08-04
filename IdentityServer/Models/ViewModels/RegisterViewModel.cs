namespace IdentityServer.Models.ViewModels
{
    public class RegisterViewModel : RegisterViewInput
    {
        public IEnumerable<string>? Errors { get; set; }
    }
}
