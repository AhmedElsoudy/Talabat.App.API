using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatApp.Core.Entities;

namespace TalabatApp.Core.Specifications
{
    public class ProductCountSpecification : BaseSpecification<Product>
    {
        public ProductCountSpecification(ProductSpecParam specParam) : base
            (
                P =>
                    (string.IsNullOrEmpty(specParam.Search) || P.Name.ToLower().Contains(specParam.Search.ToLower())) &&
                    (!specParam.BrandId.HasValue || P.BrandId == specParam.BrandId) &&
                    (!specParam.CategoryId.HasValue || P.CategoryId == specParam.CategoryId)

            )
        {
            
        }
    }
}
