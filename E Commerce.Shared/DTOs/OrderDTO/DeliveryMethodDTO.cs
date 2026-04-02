using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.DTOs.OrderDTO
{
    public record DeliveryMethodDTO
    {
        
       public int Id { get; init; }

        public string ShortName { get; init; } = default!;
        public string Descrition { get; init; } = default!;
        public string DeliveryTime { get; init; } = default!;

        public decimal Price { get; init; }

    }
}
