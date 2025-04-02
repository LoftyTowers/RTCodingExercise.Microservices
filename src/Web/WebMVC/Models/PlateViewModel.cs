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

        [Display(Name = "Promo Code Used")]
        public string? PromoCodeUsed { get; set; }

        [Display(Name = "Final Sale Price")]
        [DataType(DataType.Currency)]
        public decimal? FinalSalePrice { get; set; }

        [ScaffoldColumn(false)]
        public decimal DisplayPrice => FinalSalePrice ?? SalePriceToDisplay ?? SalePrice;

        [ScaffoldColumn(false)]
        public decimal? SalePriceToDisplay { get; set; } // preview price in views

    }
}
