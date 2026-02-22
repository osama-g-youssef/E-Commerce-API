using E_Commerce.Domain.Entities.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.Data.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(X => X.Price)
                .HasColumnType("decimal(8,2)");

            builder.OwnsOne(X => X.Product, OEntity =>
            {
                OEntity.Property(X => X.ProductName).HasMaxLength(100);
                OEntity.Property(X => X.PictureUrl).HasMaxLength(200);

            });
        }
    }
}
