namespace Northwind.Domain.Core.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Northwind.Domain.Core.Entities.NRepository_NorthwindContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Northwind.Domain.Core.Entities.NRepository_NorthwindContext context)
        {

        }
    }

}
