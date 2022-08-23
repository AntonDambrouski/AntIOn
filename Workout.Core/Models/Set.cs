namespace Workout.Core.Models;

public abstract class Set
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Exercise Exercise { get; set; }
    public int Rest { get; set; }
    public bool Max { get; set; }
}
