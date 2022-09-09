using Workout.Api.ApiModels.UrlQueries;
using Workout.Api.Wrappers;
using Workout.Core.Interfaces.Services;

namespace Workout.Api.Helpers
{
    public static class PaginationHelper
    {
        public static PagedResponse<IEnumerable<TData>> CreatePagedResponse<TData>(
            IEnumerable<TData> pagedData,
            PaginationUrlQuery query,
            long totalRecords,
            IUrlService urlService,
            string route)
        {
            var response = new PagedResponse<IEnumerable<TData>>(pagedData, query.PageNumber, query.PageSize);
            var totalPages = totalRecords / (double)query.PageSize;
            int roundedTotalPages = (int)Math.Ceiling(totalPages);
           
            response.NextPage =
                query.PageNumber >= 1 && query.PageNumber < roundedTotalPages
                ? urlService.CreatePagedUrl(query.PageNumber + 1, query.PageSize, route)
                : null;

            response.PreviousPage = 
                query.PageNumber > 1
                ? urlService.CreatePagedUrl(query.PageNumber - 1, query.PageSize, route)
                : null;

            response.FirstPage = urlService.CreatePagedUrl(1, query.PageSize, route);
            response.LastPage = urlService.CreatePagedUrl(roundedTotalPages, query.PageSize, route);
            response.TotalPages = roundedTotalPages;
            response.TotalRecords = totalRecords;

            return response;
        }
    }
}
