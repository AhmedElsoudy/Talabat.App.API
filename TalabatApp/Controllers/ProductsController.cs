using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalabatApp.Core.Entities;
using TalabatApp.Core.Repository.Contract;
using TalabatApp.Core.Specifications;

namespace TalabatApp.Controllers
{

    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productRepo;

        public ProductsController( IGenericRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var spec = new ProductWithBrandAndCategorySpecification();
            var products = await _productRepo.GetAllWithSpecAsync(spec);
            return Ok(products);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var spec = new ProductWithBrandAndCategorySpecification(id);

            var product = await _productRepo.GetWithSpecAsync(spec);
            if(product == null)
            {
                return NotFound(new {message = "Not Found" , StatusCode = 404});
            }
            return Ok(product);
        }
    }
}
