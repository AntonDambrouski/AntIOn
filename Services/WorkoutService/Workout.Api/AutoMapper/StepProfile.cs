using AutoMapper;
using Workout.Api.ApiModels.StepDTOs;
using Workout.Core.Models;

namespace Workout.Api.AutoMapper;

public class StepProfile : Profile
{
	public StepProfile()
	{
		CreateMap<Step, StepDisplayDTO>();
		CreateMap<Step, StepDetailedDTO>();
		CreateMap<StepCreateDTO, Step>();
		CreateMap<StepUpdateDTO, Step>();
	}
}
