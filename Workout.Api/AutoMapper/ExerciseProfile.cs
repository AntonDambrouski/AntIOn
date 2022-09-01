using AutoMapper;
using Workout.Api.ApiModels.ExerciseDTOs;
using Workout.Core.Models;

namespace Workout.Api.AutoMapper;

public class ExerciseProfile : Profile
{
	public ExerciseProfile()
	{
		CreateMap<Exercise, ExerciseDTO>();
		CreateMap<ExerciseCreateDTO, Exercise>();
		CreateMap<ExerciseDTO, Exercise>();
	}
}
