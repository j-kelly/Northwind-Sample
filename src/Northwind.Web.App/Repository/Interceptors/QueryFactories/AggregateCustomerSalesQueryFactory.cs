namespace Northwind.Web.App
{
    using System.Linq;
    using Northwind.Domain.Core.Entities;
    using Northwind.Web.App.Models;
    using NRepository.Core.Query;
    using NRepository.Core.Query.Interceptors.Factories;

    public class AggregateCustomerSalesQueryFactory : FactoryQuery<AggregateCustomerSales>
    {
        public override IQueryable<object> Query(IQueryRepository repository, object additionalQueryData)
        {
            var retQuery = repository.GetEntities<Customer>()
                .Select(p => new AggregateCustomerSales
                {
                    CompanyName = p.CompanyName,
                    CustomerId = p.CustomerID,
                    CombinedOrderValue = p.Orders
                        .SelectMany(p1 => p1.OrderDetails)
                        .Sum(p2 => (decimal?)p2.Quantity * p2.UnitPrice)
                });

            //var retQuery = from a in repository.GetEntities<Customer>()
            //               join b in repository.GetEntities<Order>() on a.CustomerID equals b.CustomerID
            //               select new AggregateCustomerSales
            //               {
            //                   CompanyName = a.CompanyName,
            //                   CustomerId = a.CustomerID,
            //                   CombinedOrderValue = a.Orders
            //                       .SelectMany(p1 => p1.OrderDetails)
            //                       .Sum(p2 => (decimal?)p2.Quantity * p2.UnitPrice),
            //               };

            return retQuery;
        }
    }
}
