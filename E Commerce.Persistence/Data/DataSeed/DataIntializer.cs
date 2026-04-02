using E_Commerce.Domain.Contract;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Entities.OrderModule;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.Data.DataSeed
{
    public class DataIntializer : IDataIntializer
    {
        private readonly StoreDbContext _dbContext;

        public DataIntializer(StoreDbContext dbContext)
        {
                _dbContext = dbContext;
        }

        public async Task IntializeAsync()
        {
            try
            {
                //now they are working together
                var HasProducts = await _dbContext.Products.AnyAsync();
                var HasBrands = await _dbContext.ProductBrands.AnyAsync();
                var HasTypes = await _dbContext.ProductTypes.AnyAsync();
                var HasDeliverMethods = await _dbContext.Set<DeliveryMethod>().AnyAsync();

                if (HasBrands && HasTypes && HasProducts && HasDeliverMethods) return;

                if (!HasBrands)
                {
                    await SeedDataFromJsonAsync<ProductBrand, int>("brands.json", _dbContext.ProductBrands);
                }

                if (!HasTypes)
                {
                   await SeedDataFromJsonAsync<ProductType, int>("types.json", _dbContext.ProductTypes);
                }

              await _dbContext.SaveChangesAsync(); //already made but we wrote it here to make sure that the brands and types are saved before products

                if (!HasProducts)
                {
                    await SeedDataFromJsonAsync<Product, int>("products.json", _dbContext.Products);
                }

                if(!HasDeliverMethods)
                {
                    await SeedDataFromJsonAsync<DeliveryMethod, int>("delivery.json", _dbContext.Set<DeliveryMethod>());
                }
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Data seed is failed {ex}");
            }
        }
        private async Task SeedDataFromJsonAsync<T,TKey>(string FileName,DbSet<T> dbset)where T : BaseEntity<TKey>
        {
            var FilePath = @"..\E Commerce.Persistence\Data\DataSeed\JSONFiles\" + FileName;

            if (!File.Exists(FilePath)) throw new FileNotFoundException($"Json File Not Found in Path : {FilePath}");
            try
            {
                using var DataStreams = File.OpenRead(FilePath);

                var Data = await JsonSerializer.DeserializeAsync<List<T>>(DataStreams, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

                if (Data != null)
                  await dbset.AddRangeAsync(Data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While Reading Json File : {ex}");
                return;
            }

        }
    }
}
