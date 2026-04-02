using E_Commerce.Presentation.Attrebutes;
using E_Commerce.Services_Abstraction;
using E_Commerce.Shared;
using E_Commerce.Shared.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controllers
{

    public class ProductsController : ApiBaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService) // now here I can only develop against interface 
        {
            _productService = productService;
        }

        [Authorize(Roles ="Admin")]
        [HttpGet] // swagger doesn't know it is a get verb so we should mention
        // Get : BaseUrl/api/Products
        //[RedisCache(5)]
        public async Task<ActionResult<PaginatedResult<ProductDTO>>>GetProducts([FromQuery]ProductQueryParams queryParams)
        {
            var products = await _productService.GetAllProductsAsync(queryParams);
            return Ok(products); //file json with 200 status code for products 

        }
        [HttpGet("{id}")]
        // Get : BaseUrl/api/Products/{id}
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            //throw new Exception();
            var product = await _productService.GetProductByIdAsync(id);
            return HandleResult<ProductDTO>(product);

        }
        [HttpGet("brands")]
        // Get : BaseUrl/api/Products/brands
        public async Task<ActionResult<IEnumerable<BrandDTO>>> GetAllBrands()
        {
            var Brands = await _productService.GetAllBrandsAsync();
            return Ok(Brands);
        }
        [HttpGet("types")]
        // Get : BaseUrl/api/Products/types
        public async Task<ActionResult<IEnumerable<TypeDTO>>> GetAllTypes()
        {
            var Types = await _productService.GetAllTypesAsync();
            return Ok(Types);
        }


    }
}
