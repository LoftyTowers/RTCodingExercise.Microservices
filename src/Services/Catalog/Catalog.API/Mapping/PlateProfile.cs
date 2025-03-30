using AutoMapper;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using Catalog.Domain;

namespace Catalog.API.Mapping
{
    public class PlateProfile : Profile
    {
        public PlateProfile()
        {
            CreateMap<PlateDto, Plate>().ReverseMap();
        }
    }
}
