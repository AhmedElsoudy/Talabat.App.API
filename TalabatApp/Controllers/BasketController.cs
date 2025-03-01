using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalabatApp.Core.Entities;
using TalabatApp.Core.Repository.Contract;
using TalabatApp.Dtos;
using TalabatApp.Errors;

namespace TalabatApp.Controllers
{
  
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController( IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet("{id}")] // GET : /api/Basket/id
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost]  // POST : /api/Basket
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            // mapping From DTO[CustomerBasketDto] To Model[CustomerBasket]
            var mappingBasketDto = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);

            var createOrUpdatedBasket = await _basketRepository.AddOrUpdateBasketAsync(mappingBasketDto);
            if (createOrUpdatedBasket is null) return BadRequest(new ApiErrorResponse(404));
            return Ok(basket);


        }

        [HttpDelete]  // DELETE : /api/basket
        public async Task<bool> DeleteBasket(string id)
        {
            var isDeleted = await _basketRepository.DeleteBasketAsync(id);
            return isDeleted;
        }





    }
}
