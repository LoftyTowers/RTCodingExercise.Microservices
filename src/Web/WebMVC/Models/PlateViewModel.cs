using System.ComponentModel.DataAnnotations;
using WebMVC.Enums;

namespace RTCodingExercise.Microservices.Models
{
    public class PlateViewModel
    {
        [Display(Name = "Plate ID")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Display(Name = "Registration Number")]
        public string Registration { get; set; }

        [Display(Name = "Purchase Price")]
        [DataType(DataType.Currency)]
        public decimal PurchasePrice { get; set; }

        [Display(Name = "Sale Price (inc. VAT)")]
        [DataType(DataType.Currency)]
        public decimal SalePrice { get; set; }

        [Display(Name = "Status")]
        public Status Status { get; set; }
    }
}
