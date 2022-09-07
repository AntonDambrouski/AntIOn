using AutoMapper;
using Workout.Api.ApiModels.FitnessGoalDTOs;
using Workout.Core.Models;

namespace Workout.Api.AutoMapper;

public class FitnessGoalProfile : Profile
{
	public FitnessGoalProfile()
	{
		CreateMap<FitnessGoal, FitnessGoalDisplayDTO>();
		CreateMap<FitnessGoal, FitnessGoalDetailedDTO>();
		CreateMap<FitnessGoalCreateDTO, FitnessGoal>();
		CreateMap<FitnessGoalUpdateDTO, FitnessGoal>();
	}
}