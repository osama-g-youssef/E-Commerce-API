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
    public class DeliveryMethodConfiguraiton : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(X => X.Price)
                .HasColumnType("decimal(8,2)");
            builder.Property(X => X.ShortName).HasMaxLength(50);
            builder.Property(X => X.Description).HasMaxLength(50);
            builder.Property(X => X.DeliveryTime).HasMaxLength(50);
             
        }
    }
}
