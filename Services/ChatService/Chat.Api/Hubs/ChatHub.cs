using Chat.Api.Constants;
using Chat.Api.Interfaces;
using Chat.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace Chat.Api.Hubs;

public class ChatHub : Hub<IChatHub>
{
    private static int Id = 0;
    private static readonly ConcurrentDictionary<string, User> Users = new();

    public async Task SendAnonymous(Message message)
    {
        message.SentDate = DateTime.UtcNow;
        message.From = Users.First(u => u.Value.ConnectionIds.Contains(Context.ConnectionId)).Value;
        await Clients.Group(GroupNames.Anonymous).ReceiveAnonymous(message);
    }

    [Authorize]
    public async Task SendAuthorized(Message message)
    {
        message.SentDate = DateTime.UtcNow;
        message.From = Users.First(u => u.Value.ConnectionIds.Contains(Context.ConnectionId)).Value;
        await Clients.Group(GroupNames.Authorized).ReceiveAuthorized(message);
    }

    public async Task NotificateUsersGroup(string username, string groupName, Notification notification)
    {
        await Clients.Group(GroupNames.Anonymous).NewUserNotification(notification);
    }

    public override async Task OnConnectedAsync()
    {
        var isAuthenticated = Context.User.Identity.IsAuthenticated;
        if (!isAuthenticated)
        {
            var username = $"John Snow #{Interlocked.Increment(ref Id)}";
            Users[username] = new User
            {
                Username = username,
                IsAuthenticated = isAuthenticated,
                ConnectionIds = new HashSet<string> { Context.ConnectionId }
            };

            await AddUserToGroup(Context.ConnectionId, GroupNames.Anonymous);
            var notification = new Notification { Text = $"{username} has joined this conversation!" };
            await NotificateUsersGroup(username, GroupNames.Anonymous, notification);
        }
        else
        {
            var username = $"Boby Axelrod {Interlocked.Increment(ref Id)}";
            var user = Users.GetOrAdd(username, _ => new User
            {
                IsAuthenticated = isAuthenticated,
                Username = username,
                ConnectionIds = new HashSet<string>()
            });

            await AddUserToGroup(Context.ConnectionId, GroupNames.Anonymous);
            await AddUserToGroup(Context.ConnectionId, GroupNames.Authorized);
            lock (user.ConnectionIds)
            {
                user.ConnectionIds.Add(Context.ConnectionId);
                if (user.ConnectionIds.Count == 1)
                {
                    var notification = new Notification { Text = $"{username} has joined this conversation!" };
                    NotificateUsersGroup(user.Username, GroupNames.Anonymous, notification).Wait();
                    NotificateUsersGroup(user.Username, GroupNames.Authorized, notification).Wait();
                }
            }
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var isAuthenticated = Context.User.Identity.IsAuthenticated;
        var user = Users.First(u => u.Value.ConnectionIds.Contains(Context.ConnectionId)).Value;
        if (!isAuthenticated)
        {
            Users.Remove(user.Username, out var removedUser);
            var notification = new Notification { Text = $"{user.Username} has left this conversation!" };
            await NotificateUsersGroup(removedUser.Username, GroupNames.Anonymous, notification);
        }
        else
        {
            lock (user.ConnectionIds)
            {
                user.ConnectionIds.RemoveWhere(cid => cid.Equals(Context.ConnectionId));
                if (!user.ConnectionIds.Any())
                {
                    Users.Remove(user.Username, out var removedUser);
                    var notification = new Notification { Text = $"{user.Username} has left this conversation!" };
                    NotificateUsersGroup(user.Username, GroupNames.Anonymous, notification).Wait();
                    NotificateUsersGroup(user.Username, GroupNames.Authorized, notification).Wait();
                }
            }
        }

        await base.OnDisconnectedAsync(exception);
    }

    private async Task AddUserToGroup(string connectionId, string groupName)
    {
        await Groups.AddToGroupAsync(connectionId, groupName);
    }
}
