using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatApp.Core;
using TalabatApp.Core.Entities;
using TalabatApp.Core.Order_Entities;
using TalabatApp.Core.Repository.Contract;
using TalabatApp.Core.Services.Contract;
using TalabatApp.Core.Specifications.OrderSpecification;

namespace TalabatApp.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        //private readonly IGenericRepository<Product> _productRepo;
        //private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
        //private readonly IGenericRepository<Order> _orderRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(
            IBasketRepository basketRepository,
            //IGenericRepository<Product> productRepo,
            //IGenericRepository<DeliveryMethod> deliveryMethodRepo,
            //IGenericRepository<Order> OrderRepo,
            IUnitOfWork unitOfWork,
            IPaymentService paymentService


            )
        {
            _basketRepository = basketRepository;
            //_productRepo = productRepo;
            //_deliveryMethodRepo = deliveryMethodRepo;
            //_orderRepo = OrderRepo;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, ShippingAddress shippingAddress)
        {
            // Get Basket From Basket Repo

            var basket = await _basketRepository.GetBasketAsync(basketId);

            // Get Selected Items At Basket From Products Repo

            var orderItems = new List<OrderItem>();
            if(basket?.Items?.Count() > 0)
            {
                foreach (var item in orderItems)
                {
                    var product = await _unitOfWork.Repository<Product>().GetAsync(item.Id);
                    var productItemOrder = new ProductItemOrder(item.Id, product.Name, product.PictureUrl);
                    var productItem = new OrderItem(productItemOrder, product.Price, item.Quantity);
                    orderItems.Add(productItem);

                }
            }


            // Calculate SubTotal
            var SubTotal = orderItems.Sum(orderItem => orderItem.Price * orderItem.Quantity);

            // Get Delivery Method From Delivery Method Repo
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(deliveryMethodId);

            // Create Order

            var spec = new OrderWithPaymentIntentSpec(basket.PaymentIntentId);
            var ExOrder = await _unitOfWork.Repository<Order>().GetWithSpecAsync(spec);
            if(ExOrder is not null)
            {
                _unitOfWork.Repository<Order>().DeleteAsync(ExOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basketId);

            }

            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems,basket.PaymentIntentId, SubTotal);
            await _unitOfWork.Repository<Order>().AddAsync(order);

            // Save To Database

            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0) return null;

            return order;


        }

        public async Task<Order> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            var orderRepo = _unitOfWork.Repository<Order>();
            var spec = new OrderSpecifications(orderId, buyerEmail);
            var order = await orderRepo.GetWithSpecAsync(spec);
            return order;

        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var orderRepo = _unitOfWork.Repository<Order>();
            var spec = new OrderSpecifications(buyerEmail);
            var orders = await orderRepo.GetAllWithSpecAsync(spec);
            return orders;

        }
    }
}
