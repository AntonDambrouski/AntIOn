namespace Chat.Api.Models;
public class User
{
    public string Username { get; set; }
    public bool IsAuthenticated { get; set; }
    public HashSet<string> ConnectionIds { get; set; }

    public User()
    { }

    public User(string username, bool isAuthenticated = false)
    {
        Username = username;
        IsAuthenticated = isAuthenticated;
        ConnectionIds = new HashSet<string>();
    }
}
