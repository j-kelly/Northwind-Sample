namespace Northwind.Web.App.Controllers.Customers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Northwind.Domain.Core.Entities;
    using Northwind.Web.App.Models;
    using NRepository.Core;

    public class SalesController : ApiController
    {
        private readonly IRepository _Repository;

        public SalesController(IRepository repository)
        {
            _Repository = repository;
        }

        [ActionName("SalesCategories")]
        public async Task<IEnumerable<SalesCategory>> GetSalesCategories()
        {
            // this is retrieved from the database, via sql directly through query interception
            var categories = await _Repository.GetEntitiesAsync<SalesCategory>();
            return categories;
        }

        [ActionName("UpdateCategory")]
        public string PutCategory(SalesCategory salesCategory)
        {
            // This uses a global save interception to do one of 2 things.
            // 1. Save interceptor adds an audit row to the AuditPropertyTrail 
            // 2. ModifyInterceptor updates the 'ModifiedOn' and ModifiedBy' fields before commit
            var category = _Repository.GetEntity<Category>(p => p.Id == salesCategory.Id);
            //    category.CategoryName = salesCategory.CategoryName;
            category.Description = salesCategory.CategoryDescription;
            _Repository.Modify(category);
            _Repository.Save();

            //var cat = new Category { Id = salesCategory.Id };
            //_Repository.UpdateEntityState(cat, EntityState.Unchanged);
            //cat.CategoryName = "name";
            //cat.Description = "new desc";
            //_Repository.Save();

            return "Update success";
        }

        [ActionName("InsertCategory")]
        public string PostCategory(SalesCategory salesCategory)
        {
            // Uses the Add interceptor to update the 
            // createdBy, createdOn, modifiedBy and modifiedOn fields before commit
            var category = new Category
            {
                CategoryName = salesCategory.CategoryName,
                Description = salesCategory.CategoryDescription,
            };

            _Repository.Add(category);
            _Repository.Save();

            return string.Format("Added Id: {0}", category.Id);
        }
    }
}
