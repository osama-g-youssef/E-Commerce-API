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

        // Ger all orders for user with specific email 
        Task<Result<IEnumerable<OrderToReturnDTO>>> GetAllOrdersAsync(string Email);

        // Get order by id for user with specific email
        Task<Result<OrderToReturnDTO>> GetOrderByIdAsync(Guid Id, string Email);

    }
}
