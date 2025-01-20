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
    public class DeliveryMethodConfiguarations : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {

            builder.Property(D => D.Cost)
                   .HasColumnType("decimal(18,2)");

        }
    }
}
