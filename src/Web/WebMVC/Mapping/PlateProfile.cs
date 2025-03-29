using AutoMapper;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using RTCodingExercise.Microservices.Models;

namespace RTCodingExercise.Microservices.WebMVC.Mapping
{
    public class PlateProfile : Profile
    {
        public PlateProfile()
        {
            CreateMap<PlateDto, PlateViewModel>().ReverseMap();
        }
    }
}
