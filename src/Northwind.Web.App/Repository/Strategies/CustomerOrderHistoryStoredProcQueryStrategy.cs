namespace Northwind.Web.App
{
    using System;
    using NRepository.Core.Query;
    using NRepository.EntityFramework.Query;
    using Northwind.Web.App.Models;

    public class CustomerOrderHistoryStoredProcQueryStrategy : ExecuteStoreProcedureQueryStrategy<CustomerOrderHistory>
    {
        public const string StoreProcName = "CustOrderHist {0}";

        public CustomerOrderHistoryStoredProcQueryStrategy(IQueryRepository queryRepository, string customerId)
            : base(queryRepository, StoreProcName, customerId)
        {
            if (String.IsNullOrEmpty(customerId))
                throw new ArgumentException("customerId is null or empty.", nameof(customerId));
        }
    }
}
