using E_Commerce.Domain.Entities.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Specifications.OrderSpecification
{
    public class OrderSpecification : BaseSpecifications<Order, Guid>
    {
        public OrderSpecification(string email): base(o=>o.UserEmail == email)
        {
            AddInclude(o => o.DeliveryMethod);  
            AddInclude(o => o.Items);
            AddOrderByDescinding(o => o.OrderDate);
        }

        public OrderSpecification(Guid Id, string email) : base( o=>o.UserEmail == email && o.Id==Id)
        {
            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.Items);
        }
      
    }
}
