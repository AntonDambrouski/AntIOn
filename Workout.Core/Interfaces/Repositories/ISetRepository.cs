using Workout.Core.Models;

namespace Workout.Core.Interfaces.Repositories;

public interface ISetRepository : IRepository<Set>
{
    Task<IEnumerable<Set>> GetByIdsAsync(IEnumerable<string> ids);
}
