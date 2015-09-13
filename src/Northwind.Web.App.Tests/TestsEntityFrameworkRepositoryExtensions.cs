namespace Northwind.Web.App.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq.Expressions;
    using Northwind.Web.App.Models;
    using NRepository.Core.Command;
    using NRepository.Core.Query;
    using NRepository.EntityFramework;

    // Interface extension methods are tricky to mock so we have a extension method implementation class so we can override
    // the NRepository.EntityFramework implementations
    public class TestsEntityFrameworkRepositoryExtensions : IRepositoryExtensions
    {
        public static readonly IEnumerable<SalesCategory> SalesCategories = new[]
        {
             EntityGenerator.Create<SalesCategory>(p => p.Id = 1),
             EntityGenerator.Create<SalesCategory>(p => p.Id = 2),
        };

        public static readonly IEnumerable<CustomerOrderHistory> CustomerOrderHistories = new[]
        {
             EntityGenerator.Create<CustomerOrderHistory>(p => p.ProductName = "ProductName1"),
             EntityGenerator.Create<CustomerOrderHistory>(p => p.ProductName = "ProductName2"),
        };

        public IEnumerable<T> ExecuteSqlQuery<T>(IQueryRepository repository, string sql, params object[] sqlParams)
        {
            if (typeof(T) == typeof(SalesCategory))
                return (IEnumerable<T>)SalesCategories;

            if (typeof(T) == typeof(CustomerOrderHistory))
                return (IEnumerable<T>)CustomerOrderHistories;

            throw new NotImplementedException();
        }

        public int ExecuteStoredProcudure(ICommandRepository repository, string sql, params object[] sqlParams)
        {
            throw new NotImplementedException();
        }

        public void Load<TEntity, TElement>(IQueryRepository repository, TEntity entity, Expression<Func<TEntity, IList<TElement>>> navigationProperty, params IQueryStrategy[] strategies)
            where TEntity : class
            where TElement : class
        {
            throw new NotImplementedException();
        }

        public void Load<TEntity, TElement>(IQueryRepository repository, TEntity entity, Expression<Func<TEntity, ICollection<TElement>>> navigationProperty, params IQueryStrategy[] strategies)
            where TEntity : class
            where TElement : class
        {
            throw new NotImplementedException();
        }

        public void Load<TEntity, TElement>(IQueryRepository repository, TEntity entity, Expression<Func<TEntity, TElement>> navigationProperty, params IQueryStrategy[] strategies)
            where TEntity : class
            where TElement : class
        {
            throw new NotImplementedException();
        }

        public void UpdateEntityState<TEntity>(ICommandRepository repository, TEntity entity, EntityState entityState) where TEntity : class
        {
            throw new NotImplementedException();
        }
    }
}
