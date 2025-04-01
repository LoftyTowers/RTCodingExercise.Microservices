using AutoMapper;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using RTCodingExercise.Microservices.Models;
using WebMVC.Enums;

namespace RTCodingExercise.Microservices.WebMVC.Mapping
{
    public class PlateProfile : Profile
    {
        public PlateProfile()
        {
            CreateMap<PlateDto, PlateViewModel>().ReverseMap();
            CreateMap<ProfitStatsDto, ProfitStatsViewModel>().ReverseMap();
            CreateMap<SortField, EventBus.Enums.SortField>();
            CreateMap<SortDirection, EventBus.Enums.SortDirection>();

        }
    }
}
