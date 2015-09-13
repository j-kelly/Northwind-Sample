namespace Northwind.Web.App.Tests
{
    using System.Linq;
    using Northwind.Web.App.Api;
    using Northwind.Web.App.Models;
    using NRepository.Core;
    using NRepository.TestKit;
    using NUnit.Framework;
    using Northwind.Domain.Core.Entities;
    using NRepository.Core.Query;

    [TestFixture]
    public class OrdersControllerTests
    {
        [Test]
        public void GetVeryImportantCustomersShouldReturnCorrectData()
        {
            // Arrange
            var events = new RecordedRepositoryEvents();
            var repository = new InMemoryRepository(events);
            var customerSales = Enumerable.Repeat(EntityGenerator.Create<AggregateCustomerSales>(), 20).ToList();
            customerSales.ForEach(repository.Add);

            // Act
            var ordersController = new OrdersController(repository);
            var results = ordersController.GetVeryImportantCustomers().Result;

            // Assert
            results.Count().ShouldEqual(10);
            var allStrategies = events.QueryEvents.Single().GetQueryStrategies();

            // check the strategies used in this call
            allStrategies.Count().ShouldEqual(2);
            allStrategies.First().GetType().ShouldEqual(typeof(DefaultSpecificationQueryStrategy<AggregateCustomerSales>));
            allStrategies.Second().GetType().ShouldEqual(typeof(FindVeryImportantCustomersQueryStrategy));
        }

        [Test]
        public void GetAllCustomersShouldReturnAllDataWhenDefaultValuesUsed()
        {
            // Arrange
            var interceptors = new DefaultRepositoryInterceptors(new NorthwindQueryRepositoryInterceptor());
            var repository = new InMemoryRepository(interceptors);

            for (int i = 0; i < 15; i++)
            {
                repository.Add(EntityGenerator.Create<Customer>(
                    p => p.CompanyName = p.CompanyName + i,
                    p => p.CustomerID = i.ToString(),
                    p => p.Orders.First().OrderDetails.First().Quantity = (short)i,
                    p => p.Orders.First().OrderDetails.First().UnitPrice = 100.0M));
            }

            // Act
            var ordersController = new OrdersController(repository);
            var results = ordersController.GetAllCustomers().Result;

            // Assert
            results.Count().ShouldEqual(15);
            for (int i = 0; i < 15; i++)
            {
                results.ElementAt(i).CustomerId.ShouldEqual(i.ToString());
                results.ElementAt(i).CompanyName.ShouldEqual("CompanyName" + i);
                results.ElementAt(i).CombinedOrderValue.ShouldEqual(100 * i);
            }
        }

        [Test]
        public void GetAllCustomersShouldReturnCorrectDataWhenTakeAndSkipSupplied()
        {
            // Arrange
            var interceptors = new DefaultRepositoryInterceptors(new NorthwindQueryRepositoryInterceptor());
            var repository = new InMemoryRepository(interceptors);

            for (int i = 0; i < 15; i++)
            {
                repository.Add(EntityGenerator.Create<Customer>(
                    p => p.CompanyName = p.CompanyName + i,
                    p => p.CustomerID = i.ToString(),
                    p => p.Orders.First().OrderDetails.First().Quantity = (short)i,
                    p => p.Orders.First().OrderDetails.First().UnitPrice = 100.0M));
            }

            // Act
            var ordersController = new OrdersController(repository);
            var results = ordersController.GetAllCustomers(take: 2, skip: 2).Result;

            // Assert
            results.Count().ShouldEqual(2);
            results.First().CustomerId.ShouldEqual("2");
            results.Second().CustomerId.ShouldEqual("3");
        }

        [Test]
        public void GetAllCustomersShouldReturnCorrectDataWhenOrderedByDesc()
        {
            // Arrange
            var interceptors = new DefaultRepositoryInterceptors(new NorthwindQueryRepositoryInterceptor());
            var repository = new InMemoryRepository(interceptors);

            for (int i = 0; i < 15; i++)
            {
                repository.Add(EntityGenerator.Create<Customer>(
                    p => p.CompanyName = p.CompanyName + i,
                    p => p.CustomerID = i.ToString(),
                    p => p.Orders.First().OrderDetails.First().Quantity = (short)i,
                    p => p.Orders.First().OrderDetails.First().UnitPrice = 100.0M));
            }

            // Act
            var ordersController = new OrdersController(repository);
            var results = ordersController.GetAllCustomers(take: 1, skip: 0, ascending: false).Result;

            // Assert
            results.Single().CustomerId.ShouldEqual("14");
        }


        [Test]
        public void GetAllCustomersShouldReturnCorrectDataWhenMinOrderSupplied()
        {
            // Arrange
            var interceptors = new DefaultRepositoryInterceptors(new NorthwindQueryRepositoryInterceptor());
            var repository = new InMemoryRepository(interceptors);

            for (int i = 0; i < 15; i++)
            {
                repository.Add(EntityGenerator.Create<Customer>(
                    p => p.CompanyName = p.CompanyName + i,
                    p => p.CustomerID = i.ToString(),
                    p => p.Orders.First().OrderDetails.First().Quantity = (short)i,
                    p => p.Orders.First().OrderDetails.First().UnitPrice = 100.0M));
            }

            // Act
            var ordersController = new OrdersController(repository);

            // Assert
            var results = ordersController.GetAllCustomers(minOrderValue: 100 * 10).Result;
            results.Count().ShouldEqual(5);
        }

        [Test]
        public void GetCustomerOrderHistoryShouldReturnCorrectData()
        {
            // Arrange
            var events = new RecordedRepositoryEvents();
            var repository = new InMemoryRepository(events);
                        
            // Act
            var ordersController = new OrdersController(repository);
            var result = ordersController.GetCustomerOrderHistory("NotUsed").Result;

            result.Count().ShouldEqual(TestsEntityFrameworkRepositoryExtensions.CustomerOrderHistories.Count());

            var allStrategies = events.QueryEvents.Single().GetQueryStrategies();
            allStrategies.Count().ShouldEqual(3);

            allStrategies.First().GetType().ShouldEqual(typeof(DefaultSpecificationQueryStrategy<CustomerOrderHistory>));
            allStrategies.Second().GetType().ShouldEqual(typeof(CustomerOrderHistoryStoredProcQueryStrategy));
            allStrategies.Third().GetType().ShouldEqual(typeof(ConditionalQueryStrategy));
        }

        [Test]
        public void GetCustomerOrderHistoryShouldReturnCorrectDataWhenFilteredByProductName()
        {
            // Arrange
            var repository = new InMemoryRepository();

            // Act
            var ordersController = new OrdersController(repository);
            var result = ordersController.GetCustomerOrderHistory("NotUsed", "2").Result;

            // Assert
            result.Count().ShouldEqual(1);
            result.First().ProductName.ShouldEqual("ProductName2");
        }


        // Awaiting new version NRepository.Core 2.5 (properties public on ConditionalQueryStrategy)
        [TestCase(1, 2, "CompanyName", true, null)]
        public void GetAllCustomersQueryShouldUseCorrectStrategies(int take, int skip, string orderBy, bool ascending, decimal? minOrderValue)
        {
            // Arrange
            var events = new RecordedRepositoryEvents();
            var repository = new InMemoryRepository(events);

            // Act
            var notUsed = new OrdersController(repository).GetAllCustomers(take, skip, orderBy, ascending, minOrderValue).Result;

            // Assert
            var allStrategies = events.QueryEvents.Single().GetQueryStrategies();
            allStrategies.Count().ShouldEqual(4);

            allStrategies.First().GetType().ShouldEqual(typeof(DefaultSpecificationQueryStrategy<AggregateCustomerSales>));
            allStrategies.Second().GetType().ShouldEqual(typeof(ConditionalQueryStrategy));
            allStrategies.Third().GetType().ShouldEqual(typeof(ConditionalQueryStrategy));
            allStrategies.Fourth().GetType().ShouldEqual(typeof(ConditionalQueryStrategy));
        }
    }
}
