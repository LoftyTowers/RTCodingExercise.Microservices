using System.ComponentModel.DataAnnotations;

namespace RTCodingExercise.Microservices.Models
{
    public class ProfitStatsViewModel
    {
        [Display(Name = "Total Revenue")]
        [DataType(DataType.Currency)]
        public decimal TotalRevenue { get; set; }

        [Display(Name = "Average Profit Margin")]
        [DisplayFormat(DataFormatString = "{0:P2}")] // shows percentage format
        public decimal AverageProfitMargin { get; set; }
    }
}
