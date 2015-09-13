namespace Northwind.Web.App
{
    using System;
    using System.Linq;
    using Northwind.Web.App.Models;
    using NRepository.Core.Query;

    public class FindVeryImportantCustomersQueryStrategy : QueryStrategy
    {
        public override IQueryable<T> GetQueryableEntities<T>(object additionalQueryData)
        {
            if (typeof(T) != typeof(AggregateCustomerSales))
                throw new NotSupportedException("Can only be type 'AggregateCustomerSales'");

            var query = this.QueryableRepository.GetQueryableEntities<T>(additionalQueryData);

            // At present this is rule for important customers but if it does change we only need to do it here
            var filterQuery = query.AddQueryStrategy(
              new OrderByDescendingQueryStrategy<AggregateCustomerSales>(p => p.CombinedOrderValue),
              new TakeQueryStrategy(10));

            return filterQuery;
        }
    }
}