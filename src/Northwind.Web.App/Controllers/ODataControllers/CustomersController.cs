namespace Northwind.Web.App.Controllers.ODataControllers
{
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Northwind.Domain.Core.Entities;
    using NRepository.Core;
    using NRepository.EntityFramework;
    using System.Data.Entity;
    using System.Net;
    using System.Web.OData;

    public class CustomersController : ODataController
    {
        private readonly IRepository _Repository;

        public CustomersController(IRepository repository)
        {
            _Repository = repository;
        }

        [EnableQuery]
        public IQueryable<Customer> Get()
        {
            return _Repository.GetEntities<Customer>();
        }

        [EnableQuery]
        public SingleResult<Customer> Get([FromODataUri] string customerId)
        {
            var result = _Repository.GetEntities<Customer>(new FindCustomerByIdSpecificationStrategy(customerId));
            return SingleResult.Create(result);
        }

        public async Task<IHttpActionResult> Patch([FromODataUri] string customerId, Delta<Customer> customer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = await _Repository.GetEntityAsync(new FindCustomerByIdSpecificationStrategy(customerId));
            if (entity == null)
                return NotFound();

            try
            {
                customer.Patch(entity);

                _Repository.Modify(entity);
                await _Repository.SaveAsync();

                return Updated(entity);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(customerId))
                    return NotFound();
                else
                    throw;
            }
        }

        public async Task<IHttpActionResult> Put([FromODataUri] string customerId, Customer customer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (customerId != customer.CustomerID)
                return BadRequest();

            try
            {
                if (_Repository is EntityFrameworkRepository)
                    _Repository.UpdateEntityState(customer, EntityState.Modified);
                else
                    _Repository.Modify(customer);

                await _Repository.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(customerId))
                    return NotFound();
                else
                    throw;
            }

            return Updated(customer);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] string customerId)
        {
            var customer = await _Repository.GetEntityAsync<Customer>(new FindCustomerByIdSpecificationStrategy(customerId));
            if (customer == null)
                return NotFound();

            _Repository.Delete(customer);
            await _Repository.SaveAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        private bool CustomerExists(string customerId)
        {
            var exists = _Repository.GetEntities<Customer>(new FindCustomerByIdSpecificationStrategy(customerId)).Any();
            return exists;
        }
    }
}