using AutoMapper;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

namespace Catalog.API.Mapping
{
    public class PlateProfile : Profile
    {
        public PlateProfile()
        {
            CreateMap<PlateDto, Plate>().ReverseMap();
            CreateMap<ProfitStatsDto, ProfitStats>().ReverseMap();
        }
    }
}
