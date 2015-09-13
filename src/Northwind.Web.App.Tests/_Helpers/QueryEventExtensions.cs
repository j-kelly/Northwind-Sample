namespace Northwind.Web.App.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using NRepository.Core.Events;
    using NRepository.Core.Query;

    public static class QueryEventExtensions
    {
        public static IEnumerable<IQueryStrategy> GetQueryStrategies(this RepositoryQueryEvent defaultEvent)
        {
            var queryEvent = ((SimpleRepositoryQueryEvent)defaultEvent);
            var allStrategies = ((AggregateQueryStrategy)queryEvent.QueryStrategy).Aggregates;

            return new[] { queryEvent.SpecificationStrategy }.Union(allStrategies);
        }
    }
}
