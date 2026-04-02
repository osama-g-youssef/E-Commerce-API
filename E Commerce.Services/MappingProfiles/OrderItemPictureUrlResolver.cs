using AutoMapper;
using E_Commerce.Domain.Entities.OrderModule;
using E_Commerce.Shared.DTOs.OrderDTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.MappingProfiles
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDTO, string>
    {
        private readonly IConfiguration configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDTO destination, string destMember, ResolutionContext context)
        {
            //if(string.IsNullOrEmpty(source.Product.PictureUrl))
            //{
            //    return string.Empty;
            //}
            //if(source.Product.PictureUrl.StartsWith("http")|| source.Product.PictureUrl.StartsWith(""))
            //{
            //    return source.Product.PictureUrl;
            //}
            //var BaseUrl = configuration.GetSection("URLS")["BaseUrl"];

            //if (string.IsNullOrEmpty(BaseUrl))
            //    return string.Empty;

            //var PicUrl = $"{BaseUrl}{source.Product.PictureUrl}";
            //return PicUrl;



            if (source?.Product == null || string.IsNullOrEmpty(source.Product.PictureUrl))
            {
                return string.Empty;
            }

            // If it's already an absolute URL, return as-is
            if (Uri.IsWellFormedUriString(source.Product.PictureUrl, UriKind.Absolute))
            {
                return source.Product.PictureUrl;
            }

            var baseUrl = configuration["URLS:BaseUrl"];
            if (string.IsNullOrEmpty(baseUrl))
                return source.Product.PictureUrl; // fallback to relative path if no BaseUrl configured

            baseUrl = baseUrl.TrimEnd('/');
            var picPath = source.Product.PictureUrl.TrimStart('/');

            return $"{baseUrl}/{picPath}";
        }
    }
}
