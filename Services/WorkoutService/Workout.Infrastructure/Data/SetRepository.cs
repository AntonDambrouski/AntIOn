using MongoDB.Driver;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Models;

namespace Workout.Infrastructure.Data;

public class SetRepository : RepositoryBase<Set>, ISetRepository
{
    public SetRepository(IMongoCollection<Set> collection) : base(collection)
    { }

    public async Task<IEnumerable<Set>> GetByIdsAsync(IEnumerable<string> ids)
    {
        var sets = new List<Set>();
        foreach (var id in ids)
        {
            var set = await GetByIdAsync(id);
            if (set is not null)
            {
                sets.Add(set);
            }
        }

        return sets;
    }
}
