using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalabatApp.Core.Specifications
{
    public class ProductSpecParam
    {

        public string? Sorting { get; set; }

        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }

        private const int PageMaxSize = 10;

        private int pageSize;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > PageMaxSize ? PageMaxSize : value; }
        }

        public int PageIndex { get; set; }



    }
}
