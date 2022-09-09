using Workout.Core.Models;

namespace Workout.Core.Interfaces.Services;

public interface IFitnessGoalService
{
    Task<IEnumerable<Error>?> CreateAsync(FitnessGoal fitnessGoal, IEnumerable<string> stepIds, string setId);
    Task<IEnumerable<Error>?> UpdateAsync(FitnessGoal fitnessGoal, IEnumerable<string> stepIds, string setId);
    Task DeleteAsync(string id);
    Task<FitnessGoal?> GetByIdAsync(string id);
    Task<IEnumerable<FitnessGoal>> GetAllAsync();
    Task<IEnumerable<FitnessGoal>> GetPaginatedAsync(int pageNumber, int pageSize);
    Task<long> GetRecordsCountAsync();
}
