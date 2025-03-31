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

public class SalesCommandServiceTests
{
    private readonly Mock<IPublishEndpoint> _mockPublisher;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<SalesCommandService>> _mockLogger;
    private readonly SalesCommandService _service;


    public SalesCommandServiceTests()
    {
        _mockPublisher = new Mock<IPublishEndpoint>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<SalesCommandService>>();
        _service = new SalesCommandService(_mockPublisher.Object, _mockMapper.Object, _mockLogger.Object);
    }
}
