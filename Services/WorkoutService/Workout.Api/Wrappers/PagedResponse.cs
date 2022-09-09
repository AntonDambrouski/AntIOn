namespace Workout.Api.Wrappers;

public class PagedResponse<TData> : Response<TData>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public long TotalRecords { get; set; }
    public Uri FirstPage { get; set; }
    public Uri LastPage { get; set; }
    public Uri? NextPage { get; set; }
    public Uri? PreviousPage { get; set; }

    public PagedResponse(TData data, int pageNumber, int pageSize) : base(data)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
