using MongoDB.Driver;
using Workout.Core.Constants;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Models;

namespace Workout.Infrastructure.Data;

public class TrainingRepository : ITrainingRepository
{
    private readonly IMongoCollection<Training> _trainingCollection;

    public TrainingRepository()
    {
        var connectionString = Environment.GetEnvironmentVariable(EnvironmentVariablesNames.MongoDbConnectionString);
        var client = new MongoClient(connectionString);
        var mongoDatabase = client.GetDatabase(MongoDbNames.WorkoutDataBase);
        _trainingCollection = mongoDatabase.GetCollection<Training>(MongoDbNames.TrainingsCollection);
    }

    public async Task CreateAsync(Training item)
    {
        await _trainingCollection.InsertOneAsync(item);
    }

    public async Task DeleteAsync(string id)
    {
        await _trainingCollection.DeleteOneAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Training>> GetAllAsync()
    {
        return await _trainingCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Training?> GetByIdAsync(string id)
    {
        return await _trainingCollection.Find(t => t.Id == id).FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(string id, Training item)
    {
        await _trainingCollection.ReplaceOneAsync(t => t.Id == id, item);
    }
}
