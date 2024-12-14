using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalabatApp.Core.Entities;
using TalabatApp.Core.Repository.Contract;

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
            var products = await _productRepo.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<Product>> GetProduct(int id)
        {

            var product = await _productRepo.GetAsync(id);
            if(product == null)
            {
                return NotFound(new {message = "Not Found" , StatusCode = 404});
            }
            return Ok(product);
        }
    }
}
