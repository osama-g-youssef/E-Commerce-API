using E_Commerce.Services_Abstraction;
using E_Commerce.Shared.DTOs.OrderDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controllers
{
    public class OrdersController : ApiBaseController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [Authorize]
        [HttpPost]
        //POST:baseUrl/api/Orders
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(OrderDTO orderDTO)
        {

            var result = await _orderService.CreateOrderAsync(orderDTO, GetEmailFromToken());
            return HandleResult(result);
        }
        [Authorize]
        [HttpGet]
        //GET:baseUrl/api/Orders

        public async Task<ActionResult<IEnumerable<OrderToReturnDTO>>> GetOrders()
        {

            var result = await _orderService.GetAllOrdersAsync(GetEmailFromToken());
            return HandleResult(result);
        }
        [Authorize]
        [HttpGet("{id:guid}")]
        //GET:baseUrl/api/Orders
        public async Task<ActionResult<OrderToReturnDTO>> GetOrderById(Guid id)
        {
            var result = await _orderService.GetOrderByIdAsync(id, GetEmailFromToken());
            return HandleResult(result);
        }
        [AllowAnonymous]
        [HttpGet("deliveryMethods")]
        //GET:baseUrl/api/Orders/DeliveryMethods
        public async Task<ActionResult<IEnumerable<DeliveryMethodDTO>>> GetDeliveryMethods()
        {
            var result = await _orderService.GetAllDeliveryMethodsAsync();
            return HandleResult(result);
        }
    }
    }
