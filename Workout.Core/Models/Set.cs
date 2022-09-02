using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Workout.Core.Enums;

namespace Workout.Core.Models;

public class Set
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
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
            return Rest + 60;
        }

        var secondsForExercises = ValueUnit switch
        {
            Units.Seconds => Value.Value,
            Units.Reps => Value.Value,
            Units.Minutes => Value.Value * 60,
            _ => 0
        };

        return secondsForExercises + Rest;
    }
}
