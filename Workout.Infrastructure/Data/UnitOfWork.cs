using Workout.Core.Interfaces.Repositories;

namespace Workout.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly IHttpClientFactory _clientFactory;

    public UnitOfWork(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public ISetRepository SetRepository => SetRepository ?? new SetRepository(_clientFactory);

    public IStepRepository StepRepository => StepRepository ?? new StepRepository(_clientFactory);

    public IExerciseRepository ExerciseRepository => ExerciseRepository ?? new ExerciseRepository(_clientFactory);

    public ITrainingRepository TrainingRepository => TrainingRepository ?? new TrainingRepository(_clientFactory);

    public IFitnessGoalRepository FitnessGoalRepository => FitnessGoalRepository ?? new FitnessGoalRepository(_clientFactory);
}
