namespace IdentityServer.Models.ViewModels
{
    public class RegisterViewModel : RegisterViewInput
    {
        public IEnumerable<ViewError>? Errors { get; set; }
    }
}
