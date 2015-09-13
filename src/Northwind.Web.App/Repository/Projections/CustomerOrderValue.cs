namespace Northwind.Web.App.Models
{
    public class AggregateCustomerSales
    {
        public string CustomerId { get; set; }
        public string CompanyName { get; set; }
        public decimal? CombinedOrderValue { get; set; }
    }
}
