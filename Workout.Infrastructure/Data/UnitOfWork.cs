using Workout.Core.Interfaces.Repositories;

namespace Workout.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly Lazy<SetRepository> _setRepository = new(new SetRepository());
    private readonly Lazy<StepRepository> _stepRepository = new(new StepRepository());
    private readonly Lazy<ExerciseRepository> _exerciseRepository = new(new ExerciseRepository());
    private readonly Lazy<TrainingRepository> _trainingRepository = new(new TrainingRepository());
    private readonly Lazy<FitnessGoalRepository> _fitnessGoalRepository = new(new FitnessGoalRepository());

    public ISetRepository SetRepository => _setRepository.Value;

    public IStepRepository StepRepository => _stepRepository.Value;

    public IExerciseRepository ExerciseRepository => _exerciseRepository.Value;

    public ITrainingRepository TrainingRepository => _trainingRepository.Value;

    public IFitnessGoalRepository FitnessGoalRepository => _fitnessGoalRepository.Value;
}
