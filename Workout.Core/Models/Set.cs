namespace Workout.Core.Models;

public class Set
{
    public int Id { get; set; }
    public Exercise Exercise { get; set; }
    public int? Reps { get; set; }
    public int Rest { get; set; }
    public bool MaxReps { get; set; }
}
