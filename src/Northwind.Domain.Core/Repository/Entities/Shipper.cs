using System.Collections.Generic;

namespace Northwind.Domain.Core.Entities
{
    public partial class Shipper
    {
        public Shipper()
        {
            this.Orders = new List<Order>();
        }

        public int ShipperID { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
