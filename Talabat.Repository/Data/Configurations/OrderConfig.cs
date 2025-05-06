using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data.Configurations
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(O => O.Status)
                .HasConversion(OStatus => OStatus.ToString(), OStatust => (OrderStatus)Enum.Parse(typeof(OrderStatus), OStatust));
        
            builder.Property(O => O.Subtotal)
                .HasColumnType("decimal(18,2)");

            builder.OwnsOne(O => O.ShippingAddress,X => X.WithOwner());

            builder.HasOne(O => O.DeliveryMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
