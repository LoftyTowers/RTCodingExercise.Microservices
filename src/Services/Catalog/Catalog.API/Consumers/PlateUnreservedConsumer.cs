using MassTransit;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using Catalog.API.Services;
using AutoMapper;

namespace Catalog.API.Consumers
{
    public class PlateUnreservedConsumer : IConsumer<PlateUnreservedEvent>
    {
        private readonly IReservationService _reservationService;
        private readonly ILogger<PlateUnreservedConsumer> _logger;

        public PlateUnreservedConsumer(IReservationService reservationService, ILogger<PlateUnreservedConsumer> logger)
        {
            _reservationService = reservationService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PlateUnreservedEvent> context)
        {
            try
            {
                await _reservationService.UnreservePlateAsync(context.Message.Plate.Id);
                _logger.LogInformation("Processed PlateUnreservedEvent for Plate ID: {PlateId}", context.Message.Plate.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing PlateUnreservedEvent for Plate ID: {PlateId}", context.Message.Plate.Id);
                throw;
            }
        }
    }
}