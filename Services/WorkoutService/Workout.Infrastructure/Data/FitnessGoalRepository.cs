using MongoDB.Driver;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Models;

namespace Workout.Infrastructure.Data;

public class FitnessGoalRepository : RepositoryBase<FitnessGoal>, IFitnessGoalRepository
{
    public FitnessGoalRepository(IMongoCollection<FitnessGoal> collection) : base(collection)
    { }
}
