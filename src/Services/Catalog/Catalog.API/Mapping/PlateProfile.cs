using AutoMapper;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using Catalog.Domain.Enums;

namespace Catalog.API.Mapping
{
    public class PlateProfile : Profile
    {
        public PlateProfile()
        {
            CreateMap<Plate, PlateDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (EventBus.Enums.Status)src.StatusId))
                .ForSourceMember(src => src.Status, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.AuditLogs, opt => opt.DoNotValidate())
                .ReverseMap()
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => (int)src.Status))
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.AuditLogs, opt => opt.Ignore());
            CreateMap<SortField, EventBus.Enums.SortField>().ReverseMap();
            CreateMap<SortDirection, EventBus.Enums.SortDirection>().ReverseMap();
            CreateMap<Domain.Enums.Status, EventBus.Enums.Status>().ReverseMap();
        }
    }
}
