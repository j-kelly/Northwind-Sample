namespace Northwind.Web.App
{
    using System.Linq;
    using Northwind.Web.App.Models;
    using NRepository.Core.Query;
    using NRepository.EntityFramework;
    using NRepository.Core.Query.Interceptors.Factories;

    public class SalesCategoryQueryFactory : FactoryQuery<SalesCategory>
    {
        public override IQueryable<object> Query(IQueryRepository repository, object additionalQueryData)
        {
            var retQuery = repository.ExecuteSqlQuery<SalesCategory>(
                "SELECT [CategoryID], [CategoryName], [Description] FROM [dbo].[Categories]");

            return retQuery.AsQueryable();
        }
    }
}