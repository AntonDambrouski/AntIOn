﻿namespace Workout.Core.Models;

public class FitnessGoal
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Set TargetSet { get; set; }
    public IEnumerable<Step> Steps { get; set; }
    public bool IsDone { get; set; }
}
