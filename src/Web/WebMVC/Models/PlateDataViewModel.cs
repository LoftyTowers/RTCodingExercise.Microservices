namespace RTCodingExercise.Microservices.Models
{
    public class PlateDataViewModel
    {
        public List<PlateViewModel> Plates { get; set; }
        
        public decimal TotalRevenue { get; set; }
        public decimal AverageProfitMargin { get; set; }
    }
}