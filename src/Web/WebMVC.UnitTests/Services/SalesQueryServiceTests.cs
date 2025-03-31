using Xunit;
using Moq;
using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using RTCodingExercise.Microservices.WebMVC.Services;
using RTCodingExercise.Microservices.Models;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class SalesQueryServiceTests
{
    private readonly Mock<IRequestClient<GetProfitStatsEvent>> _mockClient;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<SalesQueryService>> _mockLogger;
    private readonly SalesQueryService _service;



    public SalesQueryServiceTests()
    {
        _mockClient = new Mock<IRequestClient<GetProfitStatsEvent>>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<SalesQueryService>>();
        _service = new SalesQueryService(_mockClient.Object, _mockMapper.Object, _mockLogger.Object);
    }
}
