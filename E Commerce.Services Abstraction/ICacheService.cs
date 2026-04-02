using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.IdentityDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services_Abstraction
{
    public interface ICacheService
    {
        Task<string?> GetAsync(string CacheKey);
        Task SetAsync(string CacheKey, object CacheValue, TimeSpan TimeToLive);


    }
}
