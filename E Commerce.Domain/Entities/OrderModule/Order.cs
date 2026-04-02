using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities.OrderModule
{
    public class Order : BaseEntity<Guid>
    {
        public string UserEmail { get; set; } = default!;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public OrderAddress Address { get; set; } = default!;

        public DeliveryMethod DeliveryMethod { get; set; } = default!; // Navigation Property

        // NP +id or Entity + id or  NP + PK or Entity + PK
        public int DeliveryMethodId { get; set; } // Foreign Key Property


        public ICollection<OrderItem> Items { get; set; } = []; // this is navigation property

        public decimal Subtotal { get; set; }
        //public decimal Total { get; set; }// can be calculated in runtime or database 
        public decimal GetTotal() => Subtotal + DeliveryMethod.Price; //runtime calculation
    }
}
