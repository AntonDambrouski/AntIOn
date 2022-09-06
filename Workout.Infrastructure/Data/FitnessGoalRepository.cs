using MongoDB.Driver;
using Workout.Core.Constants;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Models;

namespace Workout.Infrastructure.Data;

public class FitnessGoalRepository : IFitnessGoalRepository
{
    private readonly IMongoCollection<FitnessGoal> _fitnessGoalsCollection;

    public FitnessGoalRepository()
    {
        var connectionString = Environment.GetEnvironmentVariable(EnvironmentVariablesNames.MongoDbConnectionString);
        var client = new MongoClient(connectionString);
        var mongoDatabase = client.GetDatabase(MongoDbNames.WorkoutDataBase);
        _fitnessGoalsCollection = mongoDatabase.GetCollection<FitnessGoal>(MongoDbNames.FitnessGoalsCollection);
    }

    public async Task CreateAsync(FitnessGoal item)
    {
        await _fitnessGoalsCollection.InsertOneAsync(item);
    }

    public async Task DeleteAsync(string id)
    {
        await _fitnessGoalsCollection.DeleteOneAsync(fg => fg.Id == id);
    }

    public async Task<IEnumerable<FitnessGoal>> GetAllAsync()
    {
        return await _fitnessGoalsCollection.Find(_ => true).ToListAsync();
    }

    public async Task<FitnessGoal?> GetByIdAsync(string id)
    {
        return await _fitnessGoalsCollection.Find(fg => fg.Id == id).FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(string id, FitnessGoal item)
    {
        await _fitnessGoalsCollection.ReplaceOneAsync(fg => fg.Id == id, item);
    }
}
