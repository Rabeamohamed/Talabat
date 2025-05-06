using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {
       
        public static async Task SeedAsync(StoreContext dbContext)
        {
            if (!dbContext.ProductBrands.Any())
            {
                var BrandData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var Brasnds = JsonSerializer.Deserialize<List<ProductBrand>>(BrandData);
                if (Brasnds?.Count > 0)
                {
                    foreach (var Brand in Brasnds)
                        await dbContext.Set<ProductBrand>().AddAsync(Brand);
                    await dbContext.SaveChangesAsync();
                }
            }
            if (!dbContext.ProductTypes.Any())
            {
                var TypeData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
                var Types = JsonSerializer.Deserialize<List<ProductType>>(TypeData);
                if (Types?.Count > 0)
                {
                    foreach (var Type in Types)
                        await dbContext.Set<ProductType>().AddAsync(Type);
                    await dbContext.SaveChangesAsync();
                }
            }
            if (!dbContext.Products.Any())
            {
                var ProductData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductData);
                if (Products?.Count > 0)
                {
                    foreach (var Product in Products)
                        await dbContext.Set<Product>().AddAsync(Product);
                }

            }
            
            if (!dbContext.DeliveryMethods.Any())
            {
                var DeliveryMethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);
                if (DeliveryMethods?.Count > 0)
                {
                    foreach (var DeliveryMethod in DeliveryMethods)
                        await dbContext.Set<DeliveryMethod>().AddAsync(DeliveryMethod);
                }

            }
            
            await dbContext.SaveChangesAsync();
        }

    }
}