namespace IdentityServer.Models.ViewModels
{
    public class ViewError
    {
        public string Name { get; set; }
        public string Message { get; set; }

        public ViewError()
        {

        }

        public ViewError(string name, string message)
        {
            Name = name;
            Message = message;
        }
    }
}
