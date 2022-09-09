using Workout.Core.Enums;
using Workout.Core.Constants;

namespace Workout.Core.Models;

public class Set : EntityBase
{
    public string Name { get; set; }
    public Exercise Exercise { get; set; }
    public int Rest { get; set; }
    public int? Value { get; set; }
    public Units ValueUnit { get; set; }
    public bool Max { get; set; }

    public int GetTimeOfSetInSeconds()
    {
        if (Max)
        {
            return Rest + WorkoutManifest.TimeOfSetOnMaxInSeconds;
        }

        var secondsForExercises = ValueUnit switch
        {
            Units.Seconds => Value.Value,
            Units.Reps => Value.Value * 3,
            Units.Minutes => Value.Value * 60,
            Units.Percentages => Value.Value / 100 * WorkoutManifest.TimeOfSetOnMaxInSeconds,
            _ => 0
        };

        return secondsForExercises + Rest;
    }
}
