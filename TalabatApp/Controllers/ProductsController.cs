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
        private readonly IGenericRepository<ProductBrand> _brandsRepo;
        private readonly IGenericRepository<ProductCategory> _categoryRepo;
        private readonly IMapper _mapper;

        public ProductsController(
            IGenericRepository<Product> productRepo,
            IGenericRepository<ProductBrand> brandsRepo,
            IGenericRepository<ProductCategory> categoryRepo,
            IMapper mapper)
        {
            _productRepo = productRepo;
            _brandsRepo = brandsRepo;
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }

        [HttpGet]

        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetAllProducts([FromQuery] ProductSpecParam specParam)
        {
            var spec = new ProductWithBrandAndCategorySpecification(specParam);
            var products = await _productRepo.GetAllWithSpecAsync(spec);
            
            return Ok( _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(products) );
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

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _brandsRepo.GetAllAsync();

            return Ok(brands);
        }


        [ProducesResponseType(typeof(ProductBrand), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet("brands/{id}")]
        public async Task<ActionResult<ProductBrand>> GetBrandById(int id)
        {
            var brand = await _brandsRepo.GetAsync(id);
            if(brand == null)
            {
                return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));
            }
            return Ok(brand);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetCategories()
        {
            var categories = await _categoryRepo.GetAllAsync();

            return Ok(categories);
        }

        [ProducesResponseType(typeof(ProductCategory), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet("categories/{id}")]
        public async Task<ActionResult<ProductCategory>> GetCategoryById(int id)
        {
            var category = await _categoryRepo.GetAsync(id);
            if (category == null)
            {
                return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));
            }
            return Ok(category);
        }



        
    }
}
