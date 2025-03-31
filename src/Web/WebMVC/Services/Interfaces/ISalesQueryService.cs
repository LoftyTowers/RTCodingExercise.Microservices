using RTCodingExercise.Microservices.Models;

public interface ISalesQueryService
{
    Task<ProfitStatsViewModel> GetProfitStatsAsync();
}
