using MongoDB.Driver;
using Workout.Core.Constants;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Models;

namespace Workout.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly Lazy<SetRepository> _setRepository = new(() =>
    {
        var _setCollection = GetMongoCollection<Set>(MongoDbNames.SetsCollection);
        return new SetRepository(_setCollection);
    });

    private readonly Lazy<StepRepository> _stepRepository = new(() =>
    {
        var _stepCollection = GetMongoCollection<Step>(MongoDbNames.StepsCollection);
        return new StepRepository(_stepCollection);
    });
    
    private readonly Lazy<ExerciseRepository> _exerciseRepository = new(() =>
    {
        var _exerciseCollection = GetMongoCollection<Exercise>(MongoDbNames.ExercisesCollection);
        return new ExerciseRepository(_exerciseCollection);
    });
    
    private readonly Lazy<TrainingRepository> _trainingRepository = new(() =>
    {
        var _trainingCollection = GetMongoCollection<Training>(MongoDbNames.TrainingsCollection);
        return new TrainingRepository(_trainingCollection);
    });
    
    private readonly Lazy<FitnessGoalRepository> _fitnessGoalRepository = new(() =>
    {
        var _fitnessGoalsCollection = GetMongoCollection<FitnessGoal>(MongoDbNames.FitnessGoalsCollection);
        return new FitnessGoalRepository(_fitnessGoalsCollection);
    });

    public ISetRepository SetRepository => _setRepository.Value;

    public IStepRepository StepRepository => _stepRepository.Value;

    public IExerciseRepository ExerciseRepository => _exerciseRepository.Value;

    public ITrainingRepository TrainingRepository => _trainingRepository.Value;

    public IFitnessGoalRepository FitnessGoalRepository => _fitnessGoalRepository.Value;

    private static IMongoCollection<TDocument> GetMongoCollection<TDocument>(string collectionName)
        where TDocument : EntityBase
    {
        var connectionString = Environment.GetEnvironmentVariable(EnvironmentVariablesNames.MongoDbConnectionString);
        var client = new MongoClient(connectionString);
        var mongoDatabase = client.GetDatabase(MongoDbNames.WorkoutDataBase);
        var collection = mongoDatabase.GetCollection<TDocument>(collectionName);
        return collection;
    }
}
