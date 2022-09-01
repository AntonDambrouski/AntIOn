using AutoMapper;
using Workout.Api.ApiModels.SetDTOs;
using Workout.Core.Models;

namespace Workout.Api.AutoMapper;

public class SetProfile : Profile
{
	public SetProfile()
	{
		CreateMap<Set, SetDisplayDTO>();
		CreateMap<Set, SetDetailedDTO>();
		CreateMap<SetCreateDTO, Set>();
	}
}
