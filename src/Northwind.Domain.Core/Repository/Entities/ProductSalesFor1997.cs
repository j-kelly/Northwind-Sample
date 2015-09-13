using System;

namespace Northwind.Domain.Core.Entities
{
    public partial class ProductSalesFor1997
    {
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public Nullable<decimal> ProductSales { get; set; }
    }
}
