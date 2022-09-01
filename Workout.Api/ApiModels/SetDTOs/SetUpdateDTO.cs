﻿using Workout.Core.Enums;
using Workout.Core.Models;

namespace Workout.Api.ApiModels.SetDTOs;

public class SetUpdateDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Exercise Exercise { get; set; }
    public int Rest { get; set; }
    public int? Value { get; set; }
    public Units ValueUnit { get; set; }
    public bool Max { get; set; }
}
