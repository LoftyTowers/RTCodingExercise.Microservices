using AutoMapper;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using Catalog.Domain.Enums;

namespace Catalog.API.Mapping
{
    public class PlateProfile : Profile
    {
        public PlateProfile()
        {
            CreateMap<Plate, PlateDto>().ReverseMap().ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => (int)src.Status));;
            CreateMap<SortField, EventBus.Enums.SortField>().ReverseMap();
            CreateMap<SortDirection, EventBus.Enums.SortDirection>().ReverseMap();
            CreateMap<Domain.Enums.Status, EventBus.Enums.Status>().ReverseMap();
        }
    }
}
