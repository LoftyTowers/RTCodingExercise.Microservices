using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.API.Consumers;
using Catalog.API.Services;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using Catalog.Domain;
using Catalog.Domain.Enums;
using Xunit;
using System.Threading;

namespace Catalog.API.UnitTests.Consumers
{
    public class GetPlatesConsumerTests
    {
        private readonly Mock<IPlateService> _plateServiceMock;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<GetPlatesConsumer>> _loggerMock;
        private readonly GetPlatesConsumer _consumer;

        public GetPlatesConsumerTests()
        {
            _plateServiceMock = new Mock<IPlateService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Plate, PlateDto>()
                    .ForMember(dest => dest.Registration, opt => opt.MapFrom(src => src.Registration));
            });
            _mapper = config.CreateMapper();

            _loggerMock = new Mock<ILogger<GetPlatesConsumer>>();
            _consumer = new GetPlatesConsumer(_loggerMock.Object, _plateServiceMock.Object, _mapper);
        }
    }
}
