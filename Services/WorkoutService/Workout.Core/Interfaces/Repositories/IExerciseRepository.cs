using Workout.Core.Models;

namespace Workout.Core.Interfaces.Repositories;

public interface IExerciseRepository : IRepository<Exercise>
{
    Task<IEnumerable<Exercise>> GetPaginatedAsync(int pageNumber, int pageSize);
    Task<long> CountAsync();
}
