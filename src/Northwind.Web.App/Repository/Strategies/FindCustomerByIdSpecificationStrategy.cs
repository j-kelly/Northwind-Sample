namespace Northwind.Web.App
{
    using System;
    using Northwind.Domain.Core.Entities;
    using NRepository.Core.Query.Specification;

    public class FindCustomerByIdSpecificationStrategy : SpecificationQueryStrategy<Customer>
    {
        public FindCustomerByIdSpecificationStrategy(string customerId)
        {
            CustomerId = customerId;
        }

        public string CustomerId
        {
            get;
            private set;
        }

        public override System.Linq.Expressions.Expression<Func<Customer, bool>> SatisfiedBy(object additionalQueryData)
        {
            return p => p.CustomerID == CustomerId;
        }
    }
}
