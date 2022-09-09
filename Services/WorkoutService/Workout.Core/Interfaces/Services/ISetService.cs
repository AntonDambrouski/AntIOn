using Workout.Core.Models;

namespace Workout.Core.Interfaces.Services;

public interface ISetService
{
    Task<IEnumerable<Error>?> CreateAsync(Set set, string exerciseId);
    Task<IEnumerable<Error>?> UpdateAsync(Set set, string exerciseId);
    Task DeleteAsync(string id);
    Task<Set?> GetByIdAsync(string id);
    Task<IEnumerable<Set>> GetAllAsync();
    Task<IEnumerable<Set>> GetPaginatedAsync(int pageNumber, int pageSize);
    Task<long> GetRecordsCountAsync();
}
