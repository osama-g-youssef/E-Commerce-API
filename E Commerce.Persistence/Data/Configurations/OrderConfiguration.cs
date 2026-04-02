using E_Commerce.Domain.Entities.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(X => X.Subtotal)
                .HasColumnType("decimal(8,2)");

            builder.OwnsOne(X=> X.Address, OEntity =>
            {
                OEntity.Property(X => X.FirstName).HasMaxLength(50).IsRequired();
                OEntity.Property(X => X.LastName).HasMaxLength(50).IsRequired();
                OEntity.Property(X => X.Street).HasMaxLength(50).IsRequired();
                OEntity.Property(X => X.City).HasMaxLength(50).IsRequired();
                OEntity.Property(X => X.Country).HasMaxLength(50).IsRequired();
            });
        //    builder
        //.HasMany(o => o.Items)
        //.WithOne() // لا توجد Navigation Property داخل OrderItem تشير للأوردر
        //.HasForeignKey("OrderId") // هذا هو العمود الذي سيتم إنشاؤه في جدول OrderItems
        //.IsRequired()
        //.OnDelete(DeleteBehavior.Cascade);
        }
    }
}
