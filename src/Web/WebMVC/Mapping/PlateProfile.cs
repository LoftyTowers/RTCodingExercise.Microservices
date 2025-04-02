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
            CreateMap<PlateViewModel, PlateDto>().ReverseMap();
            CreateMap<ProfitStatsViewModel, ProfitStatsDto>().ReverseMap();
            CreateMap<SortField, EventBus.Enums.SortField>().ReverseMap();
            CreateMap<SortDirection, EventBus.Enums.SortDirection>().ReverseMap();
            CreateMap<Status, EventBus.Enums.Status>().ReverseMap();

        }
    }
}
