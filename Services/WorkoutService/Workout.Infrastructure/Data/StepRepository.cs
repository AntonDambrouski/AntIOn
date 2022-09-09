using MongoDB.Driver;
using Workout.Core.Constants;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Models;

namespace Workout.Infrastructure.Data;

public class StepRepository : RepositoryBase<Step>, IStepRepository
{
    public StepRepository(IMongoCollection<Step> collection) : base(collection)
    { }

    public async Task<IEnumerable<Step>> GetByIdsAsync(IEnumerable<string> ids)
    {
        return await _collection.Find(step => ids.Contains(step.Id)).ToListAsync();
    }
}
