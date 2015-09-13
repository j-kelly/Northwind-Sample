namespace Northwind.Domain.Core.Migrations
{
    public partial class CreateDatabase : DbMigrationExtender
    {
        public override void Up()
        {
            CreateTables();
            SqlScript("Data.sql");

            // Stored procedures 
            SqlScript("dbo.CustOrderHist.StoredProcedure.sql");
            SqlScript("dbo.CustOrdersDetail.StoredProcedure.sql");
            SqlScript("dbo.CustOrdersOrders.StoredProcedure.sql");
            SqlScript("dbo.EmployeeSalesByCountry.StoredProcedure.sql");
            SqlScript("dbo.SalesByYear.StoredProcedure.sql");
            SqlScript("dbo.TenMostExpensiveProducts.StoredProcedure.sql");

            // Views
            /*  
                SqlScript("dbo.AlphabeticalListOfProducts.View.sql");
                SqlScript("dbo.CategorySalesFor1997.View.sql");
                SqlScript("dbo.CurrentProductList.View.sql");
                SqlScript("dbo.CustomerAndSuppliersByCity.View.sql");
                SqlScript("dbo.Invoices.View.sql");
                SqlScript("dbo.OrderDetailsExtended.View.sql");
                SqlScript("dbo.OrdersQry.View.sql");
                SqlScript("dbo.OrderSubtotals.View.sql");
                SqlScript("dbo.ProductsAboveAveragePrice.View.sql");
                SqlScript("dbo.ProductSalesFor1997.View.sql");
                SqlScript("dbo.ProductsByCategory.View.sql");
                SqlScript("dbo.QuarterlyOrders.View.sql");
                SqlScript("dbo.SalesByCategory.View.sql");
                SqlScript("dbo.SalesTotalsByAmount.View.sql");
                SqlScript("dbo.SummaryOfSalesByQuarter.View.sql");
                SqlScript("dbo.SummaryOfSalesByYear.View.sql");
             */
        }

        private void CreateTables()
        {
            CreateTable(
                            "dbo.AlphabeticalListOfProducts",
                            c => new
                            {
                                ProductID = c.Int(nullable: false),
                                ProductName = c.String(nullable: false, maxLength: 40),
                                Discontinued = c.Boolean(nullable: false),
                                CategoryName = c.String(nullable: false, maxLength: 15),
                                SupplierID = c.Int(),
                                CategoryID = c.Int(),
                                QuantityPerUnit = c.String(maxLength: 20),
                                UnitPrice = c.Decimal(precision: 18, scale: 2),
                                UnitsInStock = c.Short(),
                                UnitsOnOrder = c.Short(),
                                ReorderLevel = c.Short(),
                            })
                            .PrimaryKey(t => new { t.ProductID, t.ProductName, t.Discontinued, t.CategoryName });

            CreateTable(
                "dbo.Categories",
                c => new
                {
                    CategoryID = c.Int(nullable: false, identity: true),
                    CategoryName = c.String(nullable: false, maxLength: 15),
                    Description = c.String(),
                    Picture = c.Binary(),
                })
                .PrimaryKey(t => t.CategoryID);

            CreateTable(
                "dbo.Products",
                c => new
                {
                    ProductID = c.Int(nullable: false, identity: true),
                    ProductName = c.String(nullable: false, maxLength: 40),
                    SupplierID = c.Int(),
                    CategoryID = c.Int(),
                    QuantityPerUnit = c.String(maxLength: 20),
                    UnitPrice = c.Decimal(precision: 18, scale: 2),
                    UnitsInStock = c.Short(),
                    UnitsOnOrder = c.Short(),
                    ReorderLevel = c.Short(),
                    Discontinued = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.ProductID)
                .ForeignKey("dbo.Categories", t => t.CategoryID)
                .ForeignKey("dbo.Suppliers", t => t.SupplierID)
                .Index(t => t.SupplierID)
                .Index(t => t.CategoryID);

            CreateTable(
                "dbo.OrderDetails",
                c => new
                {
                    OrderID = c.Int(nullable: false),
                    ProductID = c.Int(nullable: false),
                    UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Quantity = c.Short(nullable: false),
                    Discount = c.Single(nullable: false),
                })
                .PrimaryKey(t => new { t.OrderID, t.ProductID })
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.OrderID)
                .Index(t => t.ProductID);

            CreateTable(
                "dbo.Orders",
                c => new
                {
                    OrderID = c.Int(nullable: false, identity: true),
                    CustomerID = c.String(maxLength: 5, fixedLength: true),
                    EmployeeID = c.Int(),
                    OrderDate = c.DateTime(),
                    RequiredDate = c.DateTime(),
                    ShippedDate = c.DateTime(),
                    ShipVia = c.Int(),
                    Freight = c.Decimal(precision: 18, scale: 2),
                    ShipName = c.String(maxLength: 40),
                    ShipAddress = c.String(maxLength: 60),
                    ShipCity = c.String(maxLength: 15),
                    ShipRegion = c.String(maxLength: 15),
                    ShipPostalCode = c.String(maxLength: 10),
                    ShipCountry = c.String(maxLength: 15),
                })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.Customers", t => t.CustomerID)
                .ForeignKey("dbo.Employees", t => t.EmployeeID)
                .ForeignKey("dbo.Shippers", t => t.ShipVia)
                .Index(t => t.CustomerID)
                .Index(t => t.EmployeeID)
                .Index(t => t.ShipVia);

            CreateTable(
                "dbo.Customers",
                c => new
                {
                    CustomerID = c.String(nullable: false, maxLength: 5, fixedLength: true),
                    CompanyName = c.String(nullable: false, maxLength: 40),
                    ContactName = c.String(maxLength: 30),
                    ContactTitle = c.String(maxLength: 30),
                    Address = c.String(maxLength: 60),
                    City = c.String(maxLength: 15),
                    Region = c.String(maxLength: 15),
                    PostalCode = c.String(maxLength: 10),
                    Country = c.String(maxLength: 15),
                    Phone = c.String(maxLength: 24),
                    Fax = c.String(maxLength: 24),
                })
                .PrimaryKey(t => t.CustomerID);

            CreateTable(
                "dbo.CustomerDemographics",
                c => new
                {
                    CustomerTypeID = c.String(nullable: false, maxLength: 10, fixedLength: true),
                    CustomerDesc = c.String(),
                })
                .PrimaryKey(t => t.CustomerTypeID);

            CreateTable(
                "dbo.Employees",
                c => new
                {
                    EmployeeID = c.Int(nullable: false, identity: true),
                    LastName = c.String(nullable: false, maxLength: 20),
                    FirstName = c.String(nullable: false, maxLength: 10),
                    Title = c.String(maxLength: 30),
                    TitleOfCourtesy = c.String(maxLength: 25),
                    BirthDate = c.DateTime(),
                    HireDate = c.DateTime(),
                    Address = c.String(maxLength: 60),
                    City = c.String(maxLength: 15),
                    Region = c.String(maxLength: 15),
                    PostalCode = c.String(maxLength: 10),
                    Country = c.String(maxLength: 15),
                    HomePhone = c.String(maxLength: 24),
                    Extension = c.String(maxLength: 4),
                    Photo = c.Binary(),
                    Notes = c.String(),
                    ReportsTo = c.Int(),
                    PhotoPath = c.String(maxLength: 255),
                })
                .PrimaryKey(t => t.EmployeeID)
                .ForeignKey("dbo.Employees", t => t.ReportsTo)
                .Index(t => t.ReportsTo);

            CreateTable(
                "dbo.Territories",
                c => new
                {
                    TerritoryID = c.String(nullable: false, maxLength: 20),
                    TerritoryDescription = c.String(nullable: false, maxLength: 50, fixedLength: true),
                    RegionID = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.TerritoryID)
                .ForeignKey("dbo.Region", t => t.RegionID, cascadeDelete: true)
                .Index(t => t.RegionID);

            CreateTable(
                "dbo.Region",
                c => new
                {
                    RegionID = c.Int(nullable: false),
                    RegionDescription = c.String(nullable: false, maxLength: 50, fixedLength: true),
                })
                .PrimaryKey(t => t.RegionID);

            CreateTable(
                "dbo.Shippers",
                c => new
                {
                    ShipperID = c.Int(nullable: false, identity: true),
                    CompanyName = c.String(nullable: false, maxLength: 40),
                    Phone = c.String(maxLength: 24),
                })
                .PrimaryKey(t => t.ShipperID);

            CreateTable(
                "dbo.Suppliers",
                c => new
                {
                    SupplierID = c.Int(nullable: false, identity: true),
                    CompanyName = c.String(nullable: false, maxLength: 40),
                    ContactName = c.String(maxLength: 30),
                    ContactTitle = c.String(maxLength: 30),
                    Address = c.String(maxLength: 60),
                    City = c.String(maxLength: 15),
                    Region = c.String(maxLength: 15),
                    PostalCode = c.String(maxLength: 10),
                    Country = c.String(maxLength: 15),
                    Phone = c.String(maxLength: 24),
                    Fax = c.String(maxLength: 24),
                    HomePage = c.String(),
                })
                .PrimaryKey(t => t.SupplierID);

            CreateTable(
                "dbo.CategorySalesFor1997",
                c => new
                {
                    CategoryName = c.String(nullable: false, maxLength: 15),
                    CategorySales = c.Decimal(precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.CategoryName);

            CreateTable(
                "dbo.CurrentProductList",
                c => new
                {
                    ProductID = c.Int(nullable: false, identity: true),
                    ProductName = c.String(nullable: false, maxLength: 40),
                })
                .PrimaryKey(t => new { t.ProductID, t.ProductName });

            CreateTable(
                "dbo.CustomerAndSuppliersByCity",
                c => new
                {
                    CompanyName = c.String(nullable: false, maxLength: 40),
                    Relationship = c.String(nullable: false, maxLength: 9),
                    City = c.String(maxLength: 15),
                    ContactName = c.String(maxLength: 30),
                })
                .PrimaryKey(t => new { t.CompanyName, t.Relationship });

            CreateTable(
                "dbo.Invoices",
                c => new
                {
                    CustomerName = c.String(nullable: false, maxLength: 40),
                    Salesperson = c.String(nullable: false, maxLength: 31),
                    OrderID = c.Int(nullable: false),
                    ShipperName = c.String(nullable: false, maxLength: 40),
                    ProductID = c.Int(nullable: false),
                    ProductName = c.String(nullable: false, maxLength: 40),
                    UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Quantity = c.Short(nullable: false),
                    Discount = c.Single(nullable: false),
                    ShipName = c.String(maxLength: 40),
                    ShipAddress = c.String(maxLength: 60),
                    ShipCity = c.String(maxLength: 15),
                    ShipRegion = c.String(maxLength: 15),
                    ShipPostalCode = c.String(maxLength: 10),
                    ShipCountry = c.String(maxLength: 15),
                    CustomerID = c.String(maxLength: 5, fixedLength: true),
                    Address = c.String(maxLength: 60),
                    City = c.String(maxLength: 15),
                    Region = c.String(maxLength: 15),
                    PostalCode = c.String(maxLength: 10),
                    Country = c.String(maxLength: 15),
                    OrderDate = c.DateTime(),
                    RequiredDate = c.DateTime(),
                    ShippedDate = c.DateTime(),
                    ExtendedPrice = c.Decimal(precision: 18, scale: 2),
                    Freight = c.Decimal(precision: 18, scale: 2),
                })
                .PrimaryKey(t => new { t.CustomerName, t.Salesperson, t.OrderID, t.ShipperName, t.ProductID, t.ProductName, t.UnitPrice, t.Quantity, t.Discount });

            CreateTable(
                "dbo.OrderDetailsExtended",
                c => new
                {
                    OrderID = c.Int(nullable: false),
                    ProductID = c.Int(nullable: false),
                    ProductName = c.String(nullable: false, maxLength: 40),
                    UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Quantity = c.Short(nullable: false),
                    Discount = c.Single(nullable: false),
                    ExtendedPrice = c.Decimal(precision: 18, scale: 2),
                })
                .PrimaryKey(t => new { t.OrderID, t.ProductID, t.ProductName, t.UnitPrice, t.Quantity, t.Discount });

            CreateTable(
                "dbo.OrdersQry",
                c => new
                {
                    OrderID = c.Int(nullable: false),
                    CompanyName = c.String(nullable: false, maxLength: 40),
                    CustomerID = c.String(maxLength: 5, fixedLength: true),
                    EmployeeID = c.Int(),
                    OrderDate = c.DateTime(),
                    RequiredDate = c.DateTime(),
                    ShippedDate = c.DateTime(),
                    ShipVia = c.Int(),
                    Freight = c.Decimal(precision: 18, scale: 2),
                    ShipName = c.String(maxLength: 40),
                    ShipAddress = c.String(maxLength: 60),
                    ShipCity = c.String(maxLength: 15),
                    ShipRegion = c.String(maxLength: 15),
                    ShipPostalCode = c.String(maxLength: 10),
                    ShipCountry = c.String(maxLength: 15),
                    Address = c.String(maxLength: 60),
                    City = c.String(maxLength: 15),
                    Region = c.String(maxLength: 15),
                    PostalCode = c.String(maxLength: 10),
                    Country = c.String(maxLength: 15),
                })
                .PrimaryKey(t => new { t.OrderID, t.CompanyName });

            CreateTable(
                "dbo.OrderSubtotals",
                c => new
                {
                    OrderID = c.Int(nullable: false),
                    Subtotal = c.Decimal(precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.OrderID);

            CreateTable(
                "dbo.ProductsAboveAveragePrice",
                c => new
                {
                    ProductName = c.String(nullable: false, maxLength: 40),
                    UnitPrice = c.Decimal(precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.ProductName);

            CreateTable(
                "dbo.ProductSalesFor1997",
                c => new
                {
                    CategoryName = c.String(nullable: false, maxLength: 15),
                    ProductName = c.String(nullable: false, maxLength: 40),
                    ProductSales = c.Decimal(precision: 18, scale: 2),
                })
                .PrimaryKey(t => new { t.CategoryName, t.ProductName });

            CreateTable(
                "dbo.ProductsByCategory",
                c => new
                {
                    CategoryName = c.String(nullable: false, maxLength: 15),
                    ProductName = c.String(nullable: false, maxLength: 40),
                    Discontinued = c.Boolean(nullable: false),
                    QuantityPerUnit = c.String(maxLength: 20),
                    UnitsInStock = c.Short(),
                })
                .PrimaryKey(t => new { t.CategoryName, t.ProductName, t.Discontinued });

            CreateTable(
                "dbo.SalesByCategory",
                c => new
                {
                    CategoryID = c.Int(nullable: false),
                    CategoryName = c.String(nullable: false, maxLength: 15),
                    ProductName = c.String(nullable: false, maxLength: 40),
                    ProductSales = c.Decimal(precision: 18, scale: 2),
                })
                .PrimaryKey(t => new { t.CategoryID, t.CategoryName, t.ProductName });

            CreateTable(
                "dbo.SalesTotalsByAmount",
                c => new
                {
                    OrderID = c.Int(nullable: false),
                    CompanyName = c.String(nullable: false, maxLength: 40),
                    SaleAmount = c.Decimal(precision: 18, scale: 2),
                    ShippedDate = c.DateTime(),
                })
                .PrimaryKey(t => new { t.OrderID, t.CompanyName });

            CreateTable(
                "dbo.SummaryOfSalesByQuarter",
                c => new
                {
                    OrderID = c.Int(nullable: false),
                    ShippedDate = c.DateTime(),
                    Subtotal = c.Decimal(precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.OrderID);

            CreateTable(
                "dbo.SummaryOfSalesByYear",
                c => new
                {
                    OrderID = c.Int(nullable: false),
                    ShippedDate = c.DateTime(),
                    Subtotal = c.Decimal(precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.OrderID);

            CreateTable(
                "dbo.CustomerCustomerDemo",
                c => new
                {
                    CustomerTypeID = c.String(nullable: false, maxLength: 10, fixedLength: true),
                    CustomerID = c.String(nullable: false, maxLength: 5, fixedLength: true),
                })
                .PrimaryKey(t => new { t.CustomerTypeID, t.CustomerID })
                .ForeignKey("dbo.CustomerDemographics", t => t.CustomerTypeID, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.CustomerID, cascadeDelete: true)
                .Index(t => t.CustomerTypeID)
                .Index(t => t.CustomerID);

            CreateTable(
                "dbo.EmployeeTerritories",
                c => new
                {
                    EmployeeID = c.Int(nullable: false),
                    TerritoryID = c.String(nullable: false, maxLength: 20),
                })
                .PrimaryKey(t => new { t.EmployeeID, t.TerritoryID })
                .ForeignKey("dbo.Employees", t => t.EmployeeID, cascadeDelete: true)
                .ForeignKey("dbo.Territories", t => t.TerritoryID, cascadeDelete: true)
                .Index(t => t.EmployeeID)
                .Index(t => t.TerritoryID);
        }
    }
}
