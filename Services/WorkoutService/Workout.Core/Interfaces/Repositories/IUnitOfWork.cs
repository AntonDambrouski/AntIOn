namespace Workout.Core.Interfaces.Repositories;

public interface IUnitOfWork
{
    ISetRepository SetRepository { get; }
    IStepRepository StepRepository { get; }
    IExerciseRepository ExerciseRepository { get; }
    ITrainingRepository TrainingRepository { get; }
    IFitnessGoalRepository FitnessGoalRepository { get; }
}
