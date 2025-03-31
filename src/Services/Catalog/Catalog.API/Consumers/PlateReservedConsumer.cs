using MassTransit;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using Catalog.API.Services;
using AutoMapper;

namespace Catalog.API.Consumers
{
    public class PlateReservedConsumer : IConsumer<PlateReservedEvent>
    {
        private readonly IReservationService _reservationService;
        private readonly ILogger<PlateReservedConsumer> _logger;

        public PlateReservedConsumer(IReservationService reservationService, ILogger<PlateReservedConsumer> logger)
        {
            _reservationService = reservationService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PlateReservedEvent> context)
        {
            try
            {
                await _reservationService.ReservePlateAsync(context.Message.Plate.Id);
                _logger.LogInformation("Processed PlateReservedEvent for Plate ID: {PlateId}", context.Message.Plate.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing PlateReservedEvent for Plate ID: {PlateId}", context.Message.Plate.Id);
                throw;
            }
        }
    }
}