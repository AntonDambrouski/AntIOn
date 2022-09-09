using Microsoft.AspNetCore.SignalR;

namespace Chat.Api.Hubs;

public class MessageHub : Hub
{
    public async Task NewMessage(string message)
    {
        await Clients.All.SendAsync("MessageReceived", message);
    }
}
