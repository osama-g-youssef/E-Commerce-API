using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.DTOs.OrderDTO
{
    public class DeliveryMethodDTO
    {
        
       public int Id { get; set; }

        public string ShortName { get; set; } = default!;
        public string Descrition { get; set; } = default!;
        public string DeliveryTime { get; set; } = default!;

        public decimal Price { get; set; }

    }
}
