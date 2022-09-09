namespace Workout.Core.Models;

public class FitnessGoal : EntityBase
{
    public string Name { get; set; }
    public Set TargetSet { get; set; }
    public IEnumerable<Step> Steps { get; set; }
    public bool IsDone => Steps.All(step => step.IsCompleted);
}
