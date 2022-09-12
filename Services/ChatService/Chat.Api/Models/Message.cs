namespace Chat.Api.Models;

public class Message
{
    public User From { get; set; }
    public string Text { get; set; }
    public DateTime SentDate { get; set; }
}
