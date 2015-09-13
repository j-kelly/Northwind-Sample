namespace Northwind.Web.App
{
    using System.Linq;
    using NRepository.Core.Query;
    using NRepository.TestKit;
    using NRepository.Core.Query.Interceptors.Factories;

    public class AccountsQueryFactory : FactoryQuery<Account>
    {
        public override IQueryable<object> Query(IQueryRepository repository, object additionalQueryData)
        {
            var names = new[] { "Bill", "Aimee", "Bob", "Ellie", "Jim", "Marc", "Susan", "Isabelle", "John" };
            var accounts = names.Select(name => EntityGenerator.Create<Account>(p => p.Name = name, p => p.NickName = name + "y"));
            return accounts.AsQueryable();
        }
    }
}
