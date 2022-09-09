namespace Workout.Core.Interfaces.Services;

public interface IUrlService
{
    Uri CreatePagedUrl(int pageNumber, int pageSize, string route);
}
