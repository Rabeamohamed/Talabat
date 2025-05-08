using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifications.Order_Spec
{
    public class OrderSpecifications : BaseSpecifications<Order>
    {

        // Constructor Used To Get Order For Users
        public OrderSpecifications(string BuyerEmail) : base(O => O.BuyerEmail == BuyerEmail)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
            AddOrderByDescending(O => O.OrderDate);
        }
        // Constructor Used To Get Order For Specific User
        public OrderSpecifications(string email,int id): base(O => O.Id == id && O.BuyerEmail == email  )
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }
        

    }
}
