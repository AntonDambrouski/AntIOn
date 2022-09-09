using MongoDB.Driver;
using Workout.Core.Constants;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Models;

namespace Workout.Infrastructure.Data;

public class TrainingRepository : RepositoryBase<Training>, ITrainingRepository
{
    private readonly IMongoCollection<Training> _trainingCollection;

    public TrainingRepository(IMongoCollection<Training> collection) : base(collection)
    { }
}
