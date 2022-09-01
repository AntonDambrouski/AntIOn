using AutoMapper;
using Workout.Api.ApiModels.TrainingDTOs;
using Workout.Core.Models;

namespace Workout.Api.AutoMapper;

public class TrainingProfile : Profile
{
    public TrainingProfile()
    {
        CreateMap<Training, TrainingDisplayDTO>();
        CreateMap<Training, TrainingDetailedDTO>();
        CreateMap<TrainingCreateDTO, Training>();
    }
}
