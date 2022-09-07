using Workout.Core.Models;

namespace Workout.Core.Interfaces.Services;

public interface ITrainingService
{
    Task<IEnumerable<Error>?> CreateAsync(Training training, IEnumerable<string> setIds);
    Task<IEnumerable<Error>?> UpdateAsync(Training training, IEnumerable<string> setIds);
    Task DeleteAsync(string id);
    Task<Training?> GetByIdAsync(string id);
    Task<IEnumerable<Training>> GetAllAsync();
}
