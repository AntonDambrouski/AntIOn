using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Workout.Core.Enums;

namespace Workout.Core.Models;

public class Training
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<Set> Sets { get; set; }
    public int Duration => Sets.Sum(set => set.GetTimeOfSetInSeconds());
}
