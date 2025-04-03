using AutoMapper;
using MassTransit;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.Models;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

namespace RTCodingExercise.Microservices.WebMVC.Services
{
    public class SalesQueryService : ISalesQueryService
    {
        
        private readonly IRequestClient<SellPlateEvent> _client;
        private readonly IMapper _mapper;
        private readonly ILogger<SalesQueryService> _logger;

        public SalesQueryService(IRequestClient<SellPlateEvent> client, IMapper mapper, ILogger<SalesQueryService> logger)
        {
            _client = client;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PlateDataViewModel> SellPlate(PlateViewModel plate)
        {
            try
            {
                var dto = _mapper.Map<PlateDto>(plate);

                _logger.LogInformation("Publishing SellPlateEvent for plate {Id} with final sale price {Price} and promo code {Promo}.", dto.Id, dto.FinalSalePrice, dto.PromoCodeUsed);

                var response = await _client.GetResponse<SellPlateCompletedEvent>(new SellPlateEvent(dto, Guid.NewGuid()));

                if ((response?.Message?.PlateData == null) || !(response.Message.PlateData.Plates?.Any() ?? false))
                {
                    _logger.LogWarning("No plates found in the response.");
                    return new PlateDataViewModel();
                }

                var plateData = _mapper.Map<PlateDataViewModel>(response?.Message?.PlateData);
                _logger.LogInformation($"Mapped {plateData.Plates.Count} plates successfully.");
                return plateData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish SellPlateEvent for plate ID: {Id}", plate.Id);
                throw;
            }
        }
    }
}