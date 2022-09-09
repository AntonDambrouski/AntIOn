using Workout.Core.Models;

namespace Workout.Core.Interfaces.Repositories;

public interface IRepository<TDocument>
    where TDocument : EntityBase
{
    Task<TDocument?> GetByIdAsync(string id);
    Task<IEnumerable<TDocument>> GetAllAsync();
    Task CreateAsync(TDocument item);
    Task UpdateAsync(string id, TDocument item);
    Task DeleteAsync(string id);
    Task<long> CountAsync();
    Task<IEnumerable<TDocument>> GetPaginatedAsync(int pageNumber, int pageSize);
}
