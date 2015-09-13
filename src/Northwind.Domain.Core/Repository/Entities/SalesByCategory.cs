using System;

namespace Northwind.Domain.Core.Entities
{
    public partial class SalesByCategory
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public Nullable<decimal> ProductSales { get; set; }
    }
}
