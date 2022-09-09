namespace Workout.Core.Models;

public class Training : EntityBase
{
    public string Name { get; set; }
    public IEnumerable<Set> Sets { get; set; }
    public int Duration => Sets.Sum(set => set.GetTimeOfSetInSeconds());
}
