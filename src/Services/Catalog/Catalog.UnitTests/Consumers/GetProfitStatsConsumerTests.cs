using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using AutoMapper;
using MassTransit;
using System.Threading.Tasks;
using Catalog.API.Consumers;
using Catalog.API.Services;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

public class GetProfitStatsConsumerTests
{
    private readonly Mock<ISalesService> _mockSalesService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<GetProfitStatsConsumer>> _mockLogger;
    private readonly GetProfitStatsConsumer _consumer;

    public GetProfitStatsConsumerTests()
    {
        _mockSalesService = new Mock<ISalesService>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<GetProfitStatsConsumer>>();
        _consumer = new GetProfitStatsConsumer(_mockSalesService.Object, _mockLogger.Object,_mockMapper.Object);
    }
}
