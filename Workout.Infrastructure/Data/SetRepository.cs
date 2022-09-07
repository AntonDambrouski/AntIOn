using Microsoft.VisualBasic;
using MongoDB.Driver;
using Workout.Core.Constants;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Models;

namespace Workout.Infrastructure.Data;

public class SetRepository : ISetRepository
{
    private readonly IMongoCollection<Set> _setCollection;

    public SetRepository()
    {
        var connectionString = Environment.GetEnvironmentVariable(EnvironmentVariablesNames.MongoDbConnectionString);
        var client = new MongoClient(connectionString);
        var mongoDatabase = client.GetDatabase(MongoDbNames.WorkoutDataBase);
        _setCollection = mongoDatabase.GetCollection<Set>(MongoDbNames.SetsCollection);
    }

    public async Task CreateAsync(Set item)
    {
        await _setCollection.InsertOneAsync(item);
    }

    public async Task DeleteAsync(string id)
    {
        await _setCollection.DeleteOneAsync(set => set.Id == id);
    }

    public async Task<IEnumerable<Set>> GetAllAsync()
    {
        return await _setCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Set?> GetByIdAsync(string id)
    {
        return await _setCollection.Find(set => set.Id == id).FirstOrDefaultAsync();
    }

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

    public async Task UpdateAsync(string id, Set item)
    {
        await _setCollection.ReplaceOneAsync(set => set.Id == id, item);
    }
}
