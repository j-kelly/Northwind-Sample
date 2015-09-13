namespace Northwind.Web.App.Controllers.ApiControllers
{
    using NRepository.Core.Query;
    using System.Linq;
    using System.Web.Http;

    // Requires: NRepository.Core.3.2
    public class AccountsController : ApiController
    {
        private readonly IQueryRepository _queryRepository;

        public AccountsController(IQueryRepository queryRepository)
        {
            _queryRepository = queryRepository;
        }

        [ActionName("SearchAccounts")]
        public object GetSearchAccounts(
                string search = "",
                int page = 1,
                int pageSize = int.MaxValue,
                string sortBy = "Name",
                bool ascending = true)
        {
            var results = _queryRepository.GetEntities<Account>(
                new MultipleTextSearchSpecificationStrategy<Account>(
                    search,
                    p => p.AccountNumber,
                    p => p.MainPhoneNumber,
                    p => p.Name,
                    p => p.NickName).OnCondition(!string.IsNullOrWhiteSpace(search)));

            var filteredResults = results.AddQueryStrategy(
                    new OrderByQueryStrategy(sortBy).OnCondition(ascending),
                    new OrderByDescendingQueryStrategy(sortBy).OnCondition(!ascending),
                    new PagingQueryStrategy(page, pageSize));

            return new
            {
                TotalCount = results.Count(),
                Accounts = filteredResults.Select(p => new { Name = p.Name }).ToArray(),
            };
        }
    }
}
