namespace IdentityServer.Models.ViewModels
{
    public class ExternalLoginModel : ExternalLoginInput
    {
        public IEnumerable<ViewError>? Errors { get; set; }
    }
}
