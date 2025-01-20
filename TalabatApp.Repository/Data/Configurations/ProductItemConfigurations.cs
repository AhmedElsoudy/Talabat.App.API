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
    public class ProductItemConfigurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {

            builder.OwnsOne(OrderItem => OrderItem.Product, Product => Product.WithOwner());

            builder.Property(OrderItem => OrderItem.Price)
                   .HasColumnType("decimal(18,2)");




        }
    }
}
