using Chat.Api.Interfaces;
using Chat.Api.Models;
using System.Collections.Concurrent;

namespace Chat.Api.Services;

public class UserService : IUserService
{
    private readonly ConcurrentDictionary<User, List<string>> Users 
        = new ConcurrentDictionary<User, List<string>>();
}
