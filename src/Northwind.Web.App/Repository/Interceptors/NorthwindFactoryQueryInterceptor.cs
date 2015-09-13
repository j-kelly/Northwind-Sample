namespace Northwind.Web.App._NRepositoryExtensions.Interceptors.QueryFactories
{
    using NRepository.Core.Query.Interceptors.Factories;
    using System.Collections.Generic;

    public class NorthwindFactoryQueryInterceptor : FactoryQueryInterceptor
    {
        private static readonly IEnumerable<IFactoryQuery> AllQueryFactories = new IFactoryQuery[]
        {
            new AccountsQueryFactory(),
            new SalesCategoryQueryFactory(),
            new AggregateCustomerSalesQueryFactory(),
        };

        public NorthwindFactoryQueryInterceptor()
            : base(AllQueryFactories)
        {
        }
    }
}