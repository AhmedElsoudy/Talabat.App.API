using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TalabatApp.Core.Order_Entities;
using TalabatApp.Core.Services.Contract;
using TalabatApp.Dtos;
using TalabatApp.Errors;

namespace TalabatApp.Controllers
{
    
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {

            var address = _mapper.Map<AddressDto, ShippingAddress>(orderDto.ShippingAddress);
            var order =  await _orderService.CreateOrderAsync(orderDto.BuyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, address);
            if (order is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(order);

        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrderForSpecUser(string buyerEmail)
        {
            var orders = await _orderService.GetOrdersForUserAsync(buyerEmail);

            return Ok(_mapper.Map<OrderToReturnDto>(orders));


        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderForSpecUser(int orderId, string buyerEmail)
        {
            var order = await _orderService.GetOrderByIdForUserAsync(orderId, buyerEmail);
            if (order is null) return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));

            return Ok(order);
        }


    }
}
