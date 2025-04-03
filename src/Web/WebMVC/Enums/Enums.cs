using System.ComponentModel.DataAnnotations;

namespace WebMVC.Enums
{
    public enum SortField
    {
        None = 0,
        SalePrice = 10,
        PurchasePrice = 20,
        Registration = 30,
        Status = 40
    }

    public enum SortDirection
    {
        None = 0,
        Ascending = 10,
        Descending = 20
    }

    public enum Status
    {
        Unknown = 0,

        Available = 10,

        Reserved = 20,

        Sold = 30
    }
}