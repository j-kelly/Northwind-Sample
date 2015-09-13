namespace Northwind.Web.App.Tests
{
    using System.Linq;
    using Northwind.Web.App.Models;
    using NRepository.Core.Query;
    using NUnit.Framework;

    [TestFixture]
    public class FindVeryImportantCustomersQueryStrategyTests
    {
        [Test]
        public void GetQueryableEntitiesShouldReturnTenHighestSalesCustomers()
        {
            var customerSale20 = EntityGenerator.Create<AggregateCustomerSales>(p => p.CombinedOrderValue = 20);
            var customerSale30 = EntityGenerator.Create<AggregateCustomerSales>(p => p.CombinedOrderValue = 30);
            var customerSale40 = EntityGenerator.Create<AggregateCustomerSales>(p => p.CombinedOrderValue = 40);

            var customerSales = Enumerable.Repeat(customerSale20, 6).ToList();
            customerSales.AddRange(Enumerable.Repeat(customerSale30, 6));
            customerSales.AddRange(Enumerable.Repeat(customerSale40, 6));

            // Act
            var result = customerSales.AddQueryStrategy(new FindVeryImportantCustomersQueryStrategy());
            result.Count().ShouldEqual(10);
            result.Where(p => p.CombinedOrderValue == 40).Count().ShouldEqual(6);
            result.Where(p => p.CombinedOrderValue == 30).Count().ShouldEqual(4);
        }
    }
}
