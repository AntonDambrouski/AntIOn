﻿using Workout.Core.Enums;

namespace Workout.Core.Models;

public class Set
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Exercise Exercise { get; set; }
    public int Rest { get; set; }
    public int? Value { get; set; }
    public Units ValueUnit { get; set; }
    public bool Max { get; set; }
}
