namespace Workout.Core.Models;

public class Training
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<Set> Sets { get; set; }
    public int Duration { get; set; }
}
