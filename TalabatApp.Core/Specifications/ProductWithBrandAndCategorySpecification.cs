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
        public ProductWithBrandAndCategorySpecification(string sort) : base()
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
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

        }

        public ProductWithBrandAndCategorySpecification(int id) : base(P => P.Id == id)
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);
        }


    }
}
