namespace Northwind.Web.App.Controllers.ODataControllers
{
    using System.Linq;
    using System.Web.OData;
    using Northwind.Web.App.Models;
    using NRepository.Core.Query;
    using System.Web.Http;

    public class AggregateCustomerSalesController : ODataController
    {
        private readonly IQueryRepository _QueryRepository;

        public AggregateCustomerSalesController(IQueryRepository repository)
        {
            _QueryRepository = repository;
        }

        [EnableQuery]
        public IQueryable<AggregateCustomerSales> Get()
        {
            return _QueryRepository.GetEntities<AggregateCustomerSales>();
        }

        [EnableQuery]
        public SingleResult<AggregateCustomerSales> Get([FromODataUri] string customerId)
        {
            var result = _QueryRepository.GetEntities<AggregateCustomerSales>(p => p.CustomerId == customerId);
            return SingleResult.Create(result);
        }
    }

}
