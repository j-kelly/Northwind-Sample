namespace Northwind.Domain.Core.Entities
{
    using MongoDB.Bson.Serialization.Attributes;
    using NRepository.MongoDb.Annotations;
    using System.Collections.Generic;

    [CollectionName("Customers")]
    public partial class Customer
    {
        public Customer()
        {
            this.Orders = new List<Order>();
            this.CustomerDemographics = new List<CustomerDemographic>();
        }

        [BsonId]
        public string CustomerID { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        [BsonIgnore]
        public virtual ICollection<CustomerDemographic> CustomerDemographics { get; set; }
    }
    
}
