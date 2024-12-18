using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatApp.Core.Entities;

namespace TalabatApp.Core.Specifications
{
    public class ProductWithBrandAndCategorySpecification : BaseSpecification<Product>
    {
        public ProductWithBrandAndCategorySpecification() : base()
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);
        }

        public ProductWithBrandAndCategorySpecification(int id) : base(P => P.Id == id)
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);
        }


    }
}
