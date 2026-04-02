using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.DTOs.OrderDTO
{
    public class OrderDTO { 
         public string BasketId { get; set; }

        public int DeliveryMethodId{get;set;}
        public AddressDTO Address { get; set; } 
    }
}
