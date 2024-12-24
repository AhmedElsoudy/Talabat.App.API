using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalabatApp.Core.Entities;
using TalabatApp.Core.Repository.Contract;
using TalabatApp.Core.Specifications;
using TalabatApp.Dtos;
using TalabatApp.Errors;

namespace TalabatApp.Controllers
{

    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;

        public ProductsController( IGenericRepository<Product> productRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            var spec = new ProductWithBrandAndCategorySpecification();
            var products = await _productRepo.GetAllWithSpecAsync(spec);
            
            return Ok( _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products) );
        }


        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var spec = new ProductWithBrandAndCategorySpecification(id);

            var product = await _productRepo.GetWithSpecAsync(spec);
            if(product == null)
            {
                return NotFound(new ApiErrorResponse(404));
            }
            return Ok(_mapper.Map<Product, ProductDto>(product));
        }
    }
}
