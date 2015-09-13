using System.Data.Entity.ModelConfiguration;

namespace Northwind.Domain.Core.Entities.Mapping
{
    public class CategorySalesFor1997Map : EntityTypeConfiguration<CategorySalesFor1997>
    {
        public CategorySalesFor1997Map()
        {
            // Primary Key
            this.HasKey(t => t.CategoryName);

            // Properties
            this.Property(t => t.CategoryName)
                .IsRequired()
                .HasMaxLength(15);

            // Table & Column Mappings
            this.ToTable("CategorySalesFor1997");
            this.Property(t => t.CategoryName).HasColumnName("CategoryName");
            this.Property(t => t.CategorySales).HasColumnName("CategorySales");
        }
    }
}
