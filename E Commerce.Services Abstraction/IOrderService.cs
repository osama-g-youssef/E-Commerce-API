using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.OrderDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services_Abstraction
{
    public interface IOrderService
    {
        //Create Order
        // Takes Order DTO , Email => Order To Return DTO
         
        Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDTO, string Email);
        // Get Delivery Methods 

        Task<Result<IEnumerable<DeliveryMethodDTO>>> GetAllDeliveryMethodsAsync();
    }
}
