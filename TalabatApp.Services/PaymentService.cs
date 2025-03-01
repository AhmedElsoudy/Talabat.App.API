using Microsoft.Extensions.Configuration;
using Stripe;
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
using Product = TalabatApp.Core.Entities.Product;

namespace TalabatApp.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration, IBasketRepository basketRepository, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId)
        {
            // Secret Key
            StripeConfiguration.ApiKey = _configuration["StripeKeys:Secretkey"];

            // Get Basket

            var Basket = await _basketRepository.GetBasketAsync(BasketId);
            if (Basket == null) return null;


            var shippingCost = 0M;
            if (Basket.DeliveryMethodId.HasValue)
            {
                var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(Basket.DeliveryMethodId.Value);
                shippingCost = DeliveryMethod.Cost;
            }

            // Total = SubTotal + DeliveryMethodCost;

            if(Basket.Items.Count > 0)
            {
                foreach (var item in Basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetAsync(item.Id);
                    if (item.Price != product.Price)
                        item.Price = product.Price;

                }
            }

            // Get SubTotal
            var subTotal = Basket.Items.Sum(item => item.Price * item.Quantity);

            // Create Payment Intent

            var Service = new PaymentIntentService();
            PaymentIntent paymentIntent;

            if (String.IsNullOrEmpty(Basket.PaymentIntentId))   // Create
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long) subTotal *100 + (long) shippingCost *100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "Card"},

                };

                paymentIntent = await Service.CreateAsync(options);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;


            }
            else  // Update
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)subTotal * 100 + (long)shippingCost * 100
                };

                paymentIntent = await Service.UpdateAsync(Basket.PaymentIntentId, options);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;

            }

            await _basketRepository.AddOrUpdateBasketAsync(Basket);

            return Basket;

        }
    }
}
