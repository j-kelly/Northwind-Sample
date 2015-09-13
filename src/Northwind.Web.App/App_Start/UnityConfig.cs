namespace Northwind.Web.App.App_Start
{
    using _NRepositoryExtensions.Interceptors.QueryFactories;
    using Microsoft.Practices.Unity;
    using MongoDB.Driver;
    using Northwind.Domain.Core.Entities;
    using NRepository.Core;
    using NRepository.Core.Query;
    using NRepository.EntityFramework;
    using NRepository.EntityFramework.Query;
    using NRepository.MongoDb;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Http;
    using Unity.WebApi;

    public static class UnityConfig
    {
        public class AggregateNorthwindRepositoryInterceptor : DefaultRepositoryInterceptors
        {
            public AggregateNorthwindRepositoryInterceptor()
            {
                QueryInterceptor = new NorthwindFactoryQueryInterceptor();
            }
        }

        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);

            // Query only
            container.RegisterType<IQueryRepository, EntityFrameworkQueryRepository>(
                new InjectionConstructor(
                    typeof(NRepository_NorthwindContext),
                    typeof(NorthwindFactoryQueryInterceptor)));

            // query and command
            container.RegisterType<IRepository, EntityFrameworkRepository>(
                new InjectionConstructor(
                    typeof(NRepository_NorthwindContext),
                    typeof(AggregateNorthwindRepositoryInterceptor)));

            // Uncomment the following lines of code to enable the CustomersController to use the MongoDb implementation of IRepository
            //SetUpMongoDbData();
            //container.RegisterType<IRepository, MongoDbUnitOfWorkRepository>(
            //    "MongoDbRepository",
            //    new InjectionConstructor(GetMongoDatabase()));

            //container.RegisterType<CustomersController>(
            //    new InjectionConstructor(
            //        new ResolvedParameter(typeof(IRepository), "MongoDbRepository")));
        }


        // This method copies the Customer objects with the orders attached to the Mongo db database
        private static void SetUpMongoDbData()
        {
            var mongoDb = GetMongoDatabase();
            mongoDb.Drop();
            mongoDb = GetMongoDatabase();

            using (var efQueryRepository = new EntityFrameworkQueryRepository(new NRepository_NorthwindContext()))
            {
                // no proxy objects please
                var dbContext = (DbContext)efQueryRepository.ObjectContext;
                dbContext.Configuration.ProxyCreationEnabled = false;

                // load customers with orders attached
                var customers = efQueryRepository.GetEntities<Customer>(
                    new EagerLoadingQueryStrategy<Customer>(
                        p => p.Orders)).ToList();

                // MongoDbRepository immediately calls the db when adding, modifying or deleting data. Use this implementation
                // to both set and get the concerns. (MongoDbUnitOfWorkRepository will only return the concerns after all the items have been saved )
                using (IRepository mongoRepository = new MongoDbRepository(mongoDb))
                {
                    // Copy Ef data to mongo db
                    customers.ForEach(customer =>
                    {
                        //var result = mongoRepository.AddWithConcern(customer)
                        mongoRepository.Add(customer);
                    });
                }
            }
        }

        private static MongoDatabase GetMongoDatabase()
        {
            var server = new MongoClient().GetServer();
            var database = server.GetDatabase("NRepository_Northwind");
            return database;
        }
    }
}