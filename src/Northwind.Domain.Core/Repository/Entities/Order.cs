using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Domain.Core.Entities
{
    public partial class Order
    {
        public Order()
        {
            this.OrderDetails = new List<OrderDetail>();
        }

        [BsonId]
        [NotMapped]
        public ObjectId Id { get; set; }

        public int OrderID { get; set; }
        public string CustomerID { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public Nullable<System.DateTime> RequiredDate { get; set; }
        public Nullable<System.DateTime> ShippedDate { get; set; }
        public Nullable<int> ShipVia { get; set; }
        public Nullable<decimal> Freight { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }

        [BsonIgnore]
        public virtual Customer Customer { get; set; }

        [BsonIgnore]
        public virtual Employee Employee { get; set; }

        [BsonIgnore]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        
        [BsonIgnore]
        public virtual Shipper Shipper { get; set; }
    }
}
