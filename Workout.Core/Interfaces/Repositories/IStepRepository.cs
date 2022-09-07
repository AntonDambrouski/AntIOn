using Workout.Core.Models;

namespace Workout.Core.Interfaces.Repositories;

public interface IStepRepository : IRepository<Step>
{
    Task<IEnumerable<Step>> GetByIdsAsync(IEnumerable<string> ids);
}
