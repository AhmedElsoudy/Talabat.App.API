using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatApp.Core.Order_Entities;

namespace TalabatApp.Repository.Data.Configurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {

            builder.Property(O => O.Status)
                   .HasConversion(
                        InputConversion => InputConversion.ToString(),
                        InputConversion => (OrderStatus) Enum.Parse(typeof(OrderStatus), InputConversion)
                   );

            builder.OwnsOne(Order => Order.ShippingAddress, ShippingAddress => ShippingAddress.WithOwner());

            builder.Property(Order => Order.SubTotal)
                   .HasColumnType("decimal(18,2)");

            builder.HasOne(O => O.DeliveryMethod)
                   .WithMany()
                   .HasForeignKey(O => O.DeliveryMethodId)
                   .OnDelete(DeleteBehavior.SetNull);


        }
    }
}
