using Microsoft.AspNetCore.WebUtilities;
using Workout.Core.Interfaces.Services;

namespace Workout.Core.Services;

public class UrlService : IUrlService
{
    private readonly string _baseUrl;

    public UrlService(string baseUrl)
    {
        _baseUrl = baseUrl;
    }

    public Uri CreatePagedUrl(int pageNumber, int pageSize, string route)
    {
        var enpointUrl = new Uri(string.Concat(_baseUrl, route));
        var queryParams = new Dictionary<string, string>
        {
            { nameof(pageSize), $"{pageSize}" },
            { nameof(pageNumber), $"{pageNumber}" }
        };

        var modifiedUrl = QueryHelpers.AddQueryString(enpointUrl.ToString(), queryParams);
        return new Uri(modifiedUrl);
    }
}
