namespace Northwind.Web.App.Tests
{
    using System.Linq;
    using Northwind.Domain.Core.Entities;
    using Northwind.Web.App.Controllers.Customers;
    using Northwind.Web.App.Models;
    using NRepository.Core;
    using NRepository.TestKit;
    using NUnit.Framework;

    [TestFixture]
    public class SalesControllerTests
    {
        [Test]
        public void UpdateCategoryShouldSaveToRepository()
        {
            // Arrange
            var events = new RecordedRepositoryEvents();
            var repository = new InMemoryRepository(events);

            var newCategory = EntityGenerator.Create<Category>(p => p.Id = 999);
            repository.Add(newCategory);
            events.Reset();

            // Act
            var salesController = new SalesController(repository);
            salesController.PutCategory(new SalesCategory
            {
                Id = newCategory.Id,
                CategoryName = "NewCat",
                CategoryDescription = "NewDescription"
            });

            // Check category values
            var savedCategory = repository.GetEntity<Category>(p => p.Id == newCategory.Id);
            savedCategory.Id.ShouldEqual(savedCategory.Id);
            savedCategory.CategoryName.ShouldEqual("NewCat");
            savedCategory.Description.ShouldEqual("NewDescription");

            // As we are using an in memory implementation we need to check that the modify and save 
            // methods are called
            events.AddedEvents.Count().ShouldEqual(0);
            events.ModifiedEvents.Count().ShouldEqual(1);
            events.DeletedEvents.Count().ShouldEqual(0);
            events.SavedEvents.Count().ShouldEqual(1);

            // If required we can drill down into each event and look at all aspects 
            // of the event. Here we are check that the same reference is updated.
            var entityModified = (Category)events.ModifiedEvents.First().Entity;
            entityModified.ShouldEqual(savedCategory);
            entityModified.ShouldEqual(newCategory);
        }

        [Test]
        public void InsertCategoryShouldSaveToRepository()
        {
            // Arrange
            var events = new RecordedRepositoryEvents();
            var repository = new InMemoryRepository(events);

            // Act
            var salesController = new SalesController(repository);
            salesController.PostCategory(new SalesCategory
            {
                CategoryName = "NewCat",
                CategoryDescription = "NewDescription"
            });

            // Check category values
            var savedCategory = repository.GetEntity<Category>(p => p.Id == 0);
            savedCategory.Id.ShouldEqual(0);
            savedCategory.CategoryName.ShouldEqual("NewCat");
            savedCategory.Description.ShouldEqual("NewDescription");

            // As we are using an in memory implementation we need to check that the modify and save 
            // methods are called
            events.AddedEvents.Count().ShouldEqual(1);
            events.ModifiedEvents.Count().ShouldEqual(0);
            events.DeletedEvents.Count().ShouldEqual(0);
            events.SavedEvents.Count().ShouldEqual(1);
        }

        [Test]
        public void GetSalesCategoriesShouldReturnDataRetrievedFromInterceptorThroughProjection()
        {
            // Arrange
            var events = new RecordedRepositoryEvents();
            var repository = new InMemoryRepository(
                events,
                new DefaultRepositoryInterceptors(new NorthwindQueryRepositoryInterceptor()));

            // Act
            var salesController = new SalesController(repository);
            var items = salesController.GetSalesCategories().Result;

            // Check we are getting the necessary data from the stored proc call.
            items.Count().ShouldEqual(2);
            items.Count().ShouldEqual(TestsEntityFrameworkRepositoryExtensions.SalesCategories.Count());
            items.First().ShouldEqual(TestsEntityFrameworkRepositoryExtensions.SalesCategories.First());
            items.Second().ShouldEqual(TestsEntityFrameworkRepositoryExtensions.SalesCategories.Second());
        }
    }
}