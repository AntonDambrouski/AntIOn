using MongoDB.Driver;
using Workout.Core.Constants;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Models;

namespace Workout.Infrastructure.Data;

public class FitnessGoalRepository : RepositoryBase<FitnessGoal>, IFitnessGoalRepository
{
    public FitnessGoalRepository(IMongoCollection<FitnessGoal> collection) : base(collection)
    {
        var connectionString = Environment.GetEnvironmentVariable(EnvironmentVariablesNames.MongoDbConnectionString);
        var client = new MongoClient(connectionString);
        var mongoDatabase = client.GetDatabase(MongoDbNames.WorkoutDataBase);
        var _fitnessGoalsCollection = mongoDatabase.GetCollection<FitnessGoal>(MongoDbNames.FitnessGoalsCollection);
    }
}
