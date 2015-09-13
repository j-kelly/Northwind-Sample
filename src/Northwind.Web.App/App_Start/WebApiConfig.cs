namespace Northwind.Web.App
{
    using System.Web.Http;
    using System.Web.OData.Builder;
    using System.Web.OData.Extensions;
    using Northwind.Domain.Core.Entities;
    using Microsoft.OData.Edm;
    using Northwind.Web.App.Models;
    using System.Linq;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //
            // Web API routes
            //
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{customerid}",
                defaults: new { customerid = RouteParameter.Optional });

            //
            // OData CRUD Routes
            //
            config.MapODataServiceRoute(
                routeName: "odata",
                routePrefix: "odata",
                model: GetModel());

            //
            // OData Read only Routes
            //
            config.MapODataServiceRoute(
                routeName: "odataReadOnly",
                routePrefix: "odata/readonly",
                model: GetReadOnlyModel());

            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

        }

        private static IEdmModel GetReadOnlyModel()
        {
            var builder = new ODataConventionModelBuilder();

            builder.EntitySet<AggregateCustomerSales>("AggregateCustomerSales");
            builder.EntityType<AggregateCustomerSales>().HasKey(p => p.CustomerId);

            return builder.GetEdmModel();
        }

        private static IEdmModel GetModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");

            // Order
            builder.EntitySet<Order>("Orders");
            builder.EntityType<Order>().Ignore(p => p.ShippedDate);
            builder.EntityType<Order>().Ignore(p => p.OrderDate);
            builder.EntityType<Order>().Ignore(p => p.RequiredDate);

            // OrderDetails
            builder.EntitySet<OrderDetail>("OrderDetails");
            builder.EntityType<OrderDetail>().Ignore(p => p.Order);
            builder.EntityType<OrderDetail>().HasKey(p => p.OrderID);
            builder.EntityType<OrderDetail>().HasKey(p => p.ProductID);

            // CustomerDemographic
            builder.EntitySet<CustomerDemographic>("CustomerDemographic");
            builder.EntityType<CustomerDemographic>().Ignore(p => p.Customers);
            builder.EntityType<CustomerDemographic>().HasKey(p => p.CustomerTypeID);

            // Employees
            builder.EntitySet<Employee>("Employees");
            builder.EntityType<Employee>().Ignore(p => p.BirthDate);
            builder.EntityType<Employee>().Ignore(p => p.HireDate);

            builder.EntitySet<Category>("Categories");
            builder.EntityType<Category>().Ignore(p => p.CreatedOn);
            builder.EntityType<Category>().Ignore(p => p.ModifiedOn);

            return builder.GetEdmModel();
        }
    }
}
