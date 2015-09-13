namespace Northwind.Domain.Core.Entities
{
    using System.Data.Entity;
    using Northwind.Domain.Core.Entities.Mapping;

    /*
      <connectionStrings>
        <add name="NRepository_Northwind" connectionString="Data Source=(LocalDb)\v11.0;;Initial Catalog=Northwind_NRepository;Integrated Security=True;MultipleActiveResultSets=True;" providerName="System.Data.SqlClient" />
        <add name="NRepository_Northwind" connectionString="Data Source=.\sqlexpress;Initial Catalog=NRepository_Northwind;Integrated Security=True;MultipleActiveResultSets=True;" providerName="System.Data.SqlClient" />
    </connectionStrings>
    */
    public class NRepository_NorthwindContext : DbContext
    {
        static NRepository_NorthwindContext()
        {
            //Database.SetInitializer<NRepository_NorthwindContext>(null);
        }

        public NRepository_NorthwindContext()
            : base("Name=NRepository_Northwind")
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<CustomerDemographic> CustomerDemographics { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Shipper> Shippers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Territory> Territories { get; set; }
        public DbSet<AlphabeticalListOfProduct> AlphabeticalListOfProducts { get; set; }
        public DbSet<CategorySalesFor1997> CategorySalesFor1997 { get; set; }
        public DbSet<CurrentProductList> CurrentProductLists { get; set; }
        public DbSet<CustomerAndSuppliersByCity> CustomerAndSuppliersByCities { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<OrderDetailsExtended> OrderDetailsExtendeds { get; set; }
        public DbSet<OrdersQry> OrdersQries { get; set; }
        public DbSet<OrderSubtotal> OrderSubtotals { get; set; }
        public DbSet<ProductsAboveAveragePrice> ProductsAboveAveragePrices { get; set; }
        public DbSet<ProductSalesFor1997> ProductSalesFor1997 { get; set; }
        public DbSet<ProductsByCategory> ProductsByCategories { get; set; }
        public DbSet<SalesByCategory> SalesByCategories { get; set; }
        public DbSet<SalesTotalsByAmount> SalesTotalsByAmounts { get; set; }
        public DbSet<SummaryOfSalesByQuarter> SummaryOfSalesByQuarters { get; set; }
        public DbSet<SummaryOfSalesByYear> SummaryOfSalesByYears { get; set; }

 
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new CustomerDemographicMap());
            modelBuilder.Configurations.Add(new CustomerMap());
            modelBuilder.Configurations.Add(new EmployeeMap());
            modelBuilder.Configurations.Add(new OrderDetailMap());
            modelBuilder.Configurations.Add(new OrderMap());
            modelBuilder.Configurations.Add(new ProductMap());
            modelBuilder.Configurations.Add(new RegionMap());
            modelBuilder.Configurations.Add(new ShipperMap());
            modelBuilder.Configurations.Add(new SupplierMap());
            modelBuilder.Configurations.Add(new TerritoryMap());
            modelBuilder.Configurations.Add(new AlphabeticalListOfProductMap());
            modelBuilder.Configurations.Add(new CategorySalesFor1997Map());
            modelBuilder.Configurations.Add(new CurrentProductListMap());
            modelBuilder.Configurations.Add(new CustomerAndSuppliersByCityMap());
            modelBuilder.Configurations.Add(new InvoiceMap());
            modelBuilder.Configurations.Add(new OrderDetailsExtendedMap());
            modelBuilder.Configurations.Add(new OrdersQryMap());
            modelBuilder.Configurations.Add(new OrderSubtotalMap());
            modelBuilder.Configurations.Add(new ProductsAboveAveragePriceMap());
            modelBuilder.Configurations.Add(new ProductSalesFor1997Map());
            modelBuilder.Configurations.Add(new ProductsByCategoryMap());
            modelBuilder.Configurations.Add(new SalesByCategoryMap());
            modelBuilder.Configurations.Add(new SalesTotalsByAmountMap());
            modelBuilder.Configurations.Add(new SummaryOfSalesByQuarterMap());
            modelBuilder.Configurations.Add(new SummaryOfSalesByYearMap());
        }
    }
}
