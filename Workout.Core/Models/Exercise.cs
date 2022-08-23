using Workout.Core.Enums;

namespace Workout.Core.Models;

public class Exercise
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ExerciseType Type { get; set; }
}
