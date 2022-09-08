using MongoDB.Driver;
using Workout.Core.Constants;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Models;

namespace Workout.Infrastructure.Data;

public class ExerciseRepository : IExerciseRepository
{
    private readonly IMongoCollection<Exercise> _exercisesCollection;

    public ExerciseRepository()
    {
        var connectionString = Environment.GetEnvironmentVariable(EnvironmentVariablesNames.MongoDbConnectionString);
        var client = new MongoClient(connectionString);
        var mongoDatabase = client.GetDatabase(MongoDbNames.WorkoutDataBase);
        _exercisesCollection = mongoDatabase.GetCollection<Exercise>(MongoDbNames.ExercisesCollection);
    }

    public async Task CreateAsync(Exercise item)
    {
        await _exercisesCollection.InsertOneAsync(item);
    }

    public async Task DeleteAsync(string id)
    {
        await _exercisesCollection.DeleteOneAsync(ex => ex.Id == id);
    }

    public async Task<IEnumerable<Exercise>> GetAllAsync()
    {
        return await _exercisesCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Exercise?> GetByIdAsync(string id)
    {
        return await _exercisesCollection.Find(ex => ex.Id == id).FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(string id, Exercise item)
    {
        await _exercisesCollection.ReplaceOneAsync(ex => ex.Id == id, item);
    }
}
