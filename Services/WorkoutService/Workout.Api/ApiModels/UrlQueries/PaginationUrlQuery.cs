using Workout.Core.Constants;

namespace Workout.Api.ApiModels.UrlQueries;

public class PaginationUrlQuery : UrlQueryBase
{
    private int _pageNumber;
    private int _pageSize;

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > ResponseProperties.MaxPageSize ? ResponseProperties.MaxPageSize : value;
    }

    public PaginationUrlQuery()
    {
        PageNumber = 1;
        PageSize = ResponseProperties.DefaultPageSize;
    }
}
