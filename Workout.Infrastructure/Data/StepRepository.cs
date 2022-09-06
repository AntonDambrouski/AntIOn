using MongoDB.Driver;
using Workout.Core.Constants;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Models;

namespace Workout.Infrastructure.Data;

public class StepRepository : IStepRepository
{
    private readonly IMongoCollection<Step> _stepsCollection;

    public StepRepository()
    {
        var connectionString = Environment.GetEnvironmentVariable(EnvironmentVariablesNames.MongoDbConnectionString);
        var client = new MongoClient(connectionString);
        var mongoDatabase = client.GetDatabase(MongoDbNames.WorkoutDataBase);
        _stepsCollection = mongoDatabase.GetCollection<Step>(MongoDbNames.StepsCollection);
    }

    public async Task CreateAsync(Step item)
    {
        await _stepsCollection.InsertOneAsync(item);
    }

    public async Task DeleteAsync(string id)
    {
        await _stepsCollection.DeleteOneAsync(step => step.Id == id);
    }

    public async Task<IEnumerable<Step>> GetAllAsync()
    {
        return await _stepsCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Step?> GetByIdAsync(string id)
    {
        return await _stepsCollection.Find(step => step.Id == id).FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(string id, Step item)
    {
        await _stepsCollection.ReplaceOneAsync(step => step.Id == id, item);
    }
}
