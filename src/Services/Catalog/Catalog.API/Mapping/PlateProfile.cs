using AutoMapper;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using Catalog.Domain.Enums;

namespace Catalog.API.Mapping
{
    public class PlateProfile : Profile
    {
        public PlateProfile()
        {
            CreateMap<PlateDto, Plate>().ReverseMap();
            CreateMap<ProfitStatsDto, ProfitStats>().ReverseMap();
            CreateMap<SortField, EventBus.Enums.SortField>();
            CreateMap<SortDirection, EventBus.Enums.SortDirection>();
        }
    }
}
