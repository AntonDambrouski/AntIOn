using MongoDB.Driver;
using Workout.Core.Constants;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Models;

namespace Workout.Infrastructure.Data;

public class ExerciseRepository : RepositoryBase<Exercise>, IExerciseRepository
{
    public ExerciseRepository(IMongoCollection<Exercise> collection) : base(collection)
    { }
}
