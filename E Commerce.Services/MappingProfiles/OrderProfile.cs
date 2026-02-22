using AutoMapper;
using E_Commerce.Domain.Entities.OrderModule;
using E_Commerce.Shared.DTOs.OrderDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.MappingProfiles
{
    public class OrderProfile :Profile
    {
        public OrderProfile()
        {
            CreateMap<AddressDTO, OrderAddress>().ReverseMap();
            CreateMap<Order, OrderToReturnDTO>()
                .ForMember(dest => dest.DeliveryMethod, opt => opt.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(dest => dest.PruductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<OrderItemPictureUrlResolver>());




        }
    }
}
