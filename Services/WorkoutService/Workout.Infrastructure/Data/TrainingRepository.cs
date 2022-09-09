using MongoDB.Driver;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Models;

namespace Workout.Infrastructure.Data;

public class TrainingRepository : RepositoryBase<Training>, ITrainingRepository
{
    public TrainingRepository(IMongoCollection<Training> collection) : base(collection)
    { }
}
