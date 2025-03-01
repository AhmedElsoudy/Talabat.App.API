using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatApp.Core.Order_Entities;

namespace TalabatApp.Core.Specifications.OrderSpecification
{
    public class OrderSpecifications : BaseSpecification<Order>
    {
        public OrderSpecifications(string buyerEmail) : base(O => O.BuyerEmail == buyerEmail) 
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);

            AddOrderBy(O => O.OrderDate);

        }

        public OrderSpecifications(int orderId, string buyerEmail)
            : base(O => O.Id == orderId && O.BuyerEmail == buyerEmail)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }
    }
}
