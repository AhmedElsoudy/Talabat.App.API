using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatApp.Core.Entities;

namespace TalabatApp.Repository.Data.Configurations
{
    public class ProductCategoryConfigurations : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.Property(C => C.Name).IsRequired();
        }
    }
}
