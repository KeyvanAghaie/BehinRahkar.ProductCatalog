using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Product : BaseEntity
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public byte[] Photo { get; set; }

        [Range(0, 999.99)]
        public decimal Price { get; set; }
       
    }


    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Code).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
            builder.Property(p => p.Price).IsRequired();
        }
    }
}
