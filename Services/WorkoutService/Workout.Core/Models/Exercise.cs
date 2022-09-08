using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Workout.Core.Enums;

namespace Workout.Core.Models;

public class Exercise
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Name { get; set; }
}
