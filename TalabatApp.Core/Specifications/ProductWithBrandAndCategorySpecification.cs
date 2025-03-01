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
        public ProductWithBrandAndCategorySpecification(ProductSpecParam specParam) : base
            (

              P =>
                (string.IsNullOrEmpty(specParam.Search) || P.Name.ToLower().Contains(specParam.Search.ToLower())) &&
                (!specParam.BrandId.HasValue || P.BrandId == specParam.BrandId) &&
                (!specParam.CategoryId.HasValue || P.CategoryId == specParam.CategoryId)
     
            )
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);

            if (!string.IsNullOrEmpty(specParam.Sorting))
            {
                switch (specParam.Sorting)
                {
                    case "priceAsc":
                        // OrderBy(P => P.Name)
                        AddOrderBy(P => P.Price);
                        break;
                    case "priceDesc":
                        // OrderByDesc(P => P.Name)
                        AddOrderByDesc(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;

                }
            }
            else
            {
                AddOrderBy(P => P.Name);
            }


            // TotalProducts = 40   5 
            // PageSize = 8
            // PageIndex = 3


            ApplyPagination((specParam.PageIndex -1) * specParam.PageSize, specParam.PageSize);

       }

        public ProductWithBrandAndCategorySpecification(int id) : base(P => P.Id == id)
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);
        }


    }
}
