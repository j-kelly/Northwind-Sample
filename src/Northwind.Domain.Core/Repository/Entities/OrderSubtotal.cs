using System;

namespace Northwind.Domain.Core.Entities
{
    public partial class OrderSubtotal
    {
        public int OrderID { get; set; }
        public Nullable<decimal> Subtotal { get; set; }
    }
}
