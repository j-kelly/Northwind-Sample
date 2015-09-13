namespace Northwind.Web.App.Api
{
    using Northwind.Web.App.Models;
    using NRepository.Core;
    using NRepository.Core.Query;
    using NRepository.Core.Query.Specification;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;

    /// <summary>
    /// A readonly controller which uses IQueryRepository and projected entities created through 
    /// the NRepository interception mechanism.
    /// </summary>
    public class OrdersController : ApiController
    {
        private readonly IQueryRepository _QueryRepository;

        public OrdersController(IQueryRepository queryRepository)
        {
            _QueryRepository = queryRepository;
        }

        [ActionName("ImportantCustomers")]
        public async Task<IEnumerable<AggregateCustomerSales>> GetVeryImportantCustomers()
        {
            // Using strategies mean we can code displaying intention rather than the implementation
            var customers = await _QueryRepository.GetEntitiesAsync<AggregateCustomerSales>(
                new FindVeryImportantCustomersQueryStrategy());

            return customers;
        }

        // Uses conditional strategies
        [ActionName("AllCustomers")]
        public async Task<IEnumerable<AggregateCustomerSales>> GetAllCustomers(
            [FromUri]int take = 0,
            [FromUri]int skip = 0,
            [FromUri]string orderBy = null,
            [FromUri]bool ascending = true,
            [FromUri]decimal? minOrderValue = null)
        {
            if (orderBy == null || typeof(AggregateCustomerSales).GetProperty(orderBy) == null)
                orderBy = PropertyInfo<AggregateCustomerSales>.GetMemberName(p => p.CombinedOrderValue);

            // using conditional strategies
            var minOrderStrategy = new ExpressionSpecificationQueryStrategy<AggregateCustomerSales>(p => p.CombinedOrderValue >= minOrderValue);
            var customers = await _QueryRepository.GetEntitiesAsync<AggregateCustomerSales>(
                minOrderStrategy.OnCondition(minOrderValue != null),
                new ConditionalQueryStrategy(ascending, () => new OrderByQueryStrategy(orderBy)),
                new ConditionalQueryStrategy(!ascending, () => new OrderByDescendingQueryStrategy(orderBy)),
                new ConditionalQueryStrategy(take > 0, () => new PagingQueryStrategy(skip < 0 ? 0 : skip, take)));

            return customers;
        }

        // uses stored procs
        [ActionName("CustomerOrderHistory")]
        public async Task<IEnumerable<CustomerOrderHistory>> GetCustomerOrderHistory(string customerid, [FromUri]string productNameContains = null)
        {
            var orderHistory = await _QueryRepository.GetEntitiesAsync<CustomerOrderHistory>(
                new CustomerOrderHistoryStoredProcQueryStrategy(_QueryRepository, customerid/*, productNameContains*/),// "VINET",
                new ConditionalQueryStrategy(
                    productNameContains != null,
                    new TextSearchSpecificationStrategy<CustomerOrderHistory>(p => p.ProductName, productNameContains, true)));

            return orderHistory;
        }
    }
}
