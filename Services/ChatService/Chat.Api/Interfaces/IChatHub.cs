using Chat.Api.Models;

namespace Chat.Api.Interfaces;

public interface IChatHub
{
    Task ReceiveAnonymous(Message message);
    Task ReceiveAuthorized(Message message);
    Task NewUserNotification(Notification notification);
}
