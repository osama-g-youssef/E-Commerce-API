using AutoMapper;
using E_Commerce.Domain.Contract;
using E_Commerce.Domain.Entities.OrderModule;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Services.Specifications.OrderSpecification;
using E_Commerce.Services_Abstraction;
using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.OrderDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IBasketRepository basketRepository;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IBasketRepository basketRepository)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.basketRepository = basketRepository;
        }
        public async Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDTO, string Email)
        {
           
            var OrderAddress = mapper.Map<AddressDTO,OrderAddress>(orderDTO.Address);
            


            var Basket =await basketRepository.GetBasketAsync(orderDTO.BasketId);
            if (Basket == null) return Error.NotFound("Basket not found.", $"Basket with Id {orderDTO.BasketId} is not found ");

            List<OrderItem> OrderItems = new List<OrderItem>();
            foreach (var item in Basket.Items)
            {
                var Product = await unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id);
                if (Product == null) return Error.NotFound("Basket not found.", $"Product with Id {item.Id} is not found ");
                OrderItems.Add(CreateOrderItem(item, Product));


            }
            var DeliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDTO.DeliveryMethodId);
            if (DeliveryMethod == null) return Error.NotFound("Delivery Method not found.", $"Delivery Method with Id {orderDTO.DeliveryMethodId} is not found ");

            var SubTotal= OrderItems.Sum(I=>I.Price * I.Quantity);
            var Order = new Order
            {
                UserEmail = Email,
                Address = OrderAddress,
                DeliveryMethod = DeliveryMethod,
                Subtotal = SubTotal,
                Items = OrderItems
            };
            await unitOfWork.GetRepository<Order, Guid>().AddAsync(Order);
            var Result = await unitOfWork.SaveChangesAsync() >0 ;
            if(!Result ) return Error.Failure("Order Creation Failed", "Failed to create order due to an internal error.");
            return mapper.Map<OrderToReturnDTO>(Order);
        }

        private static OrderItem CreateOrderItem(Domain.Entities.BasketModule.BasketItem item, Product Product)
        {
            return new OrderItem()
            {
                Product = new ProductItemOrdered
                {
                    ProductId = Product.Id,
                    ProductName = Product.Name,
                    PictureUrl = Product.PictureUrl
                },
                Price = Product.Price,
                Quantity = item.Quantity
            };
        }

        public async Task<Result<IEnumerable<DeliveryMethodDTO>>> GetAllDeliveryMethodsAsync()
        {
            var deliveryMethods = await unitOfWork.GetRepository<DeliveryMethod,int>().GetAllAsync();
            if(!deliveryMethods.Any())
                return Error.NotFound("No Delivery Methods Found", "There are no delivery methods available at the moment.");

            var deliveryMethodDTOs = mapper.Map<IEnumerable<DeliveryMethod>, IEnumerable<DeliveryMethodDTO>>(deliveryMethods);
            if(deliveryMethodDTOs == null)
                return Error.NotFound("No Delivery Methods Found", "There are no delivery methods available at the moment.");

            return Result<IEnumerable<DeliveryMethodDTO>>.Ok(deliveryMethodDTOs);
        }

        public async Task<Result<IEnumerable<OrderToReturnDTO>>> GetAllOrdersAsync(string Email)
        {
            var OrderSpec = new OrderSpecification(Email);
            var Orders = await unitOfWork.GetRepository<Order, Guid>().GetAllAsync(OrderSpec);

            if (!Orders.Any())
                return Error.NotFound(
               "No Orders Found",$"No orders found fo rthe user with : {Email}"
                    );
            var Data = mapper.Map<IEnumerable<Order>, IEnumerable<OrderToReturnDTO>>(Orders);
            return Result<IEnumerable<OrderToReturnDTO>>.Ok(Data);
        }

        public async Task<Result<OrderToReturnDTO>> GetOrderByAsync(Guid Id, string Email)
        {
            var OrderSpec = new OrderSpecification(Id, Email);
            var Order = await unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(OrderSpec);
            if (Order is null) 
                return Error.NotFound(
               "No Orders Found", $"No orders found for the user with : {Email} and order id : {Id}"
                    );
            var Data = mapper.Map<Order, OrderToReturnDTO>(Order);
            return Result<OrderToReturnDTO>.Ok(Data);

        }
    }
}
