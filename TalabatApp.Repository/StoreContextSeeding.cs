using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TalabatApp.Core.Entities;
using TalabatApp.Core.Order_Entities;
using TalabatApp.Repository.Data;

namespace TalabatApp.Repository
{
    public static class StoreContextSeeding
    {


        public static async Task SeedAsync(StoreContext _context)
        {



            #region Seeding Brands Data

            var brandsData = File.ReadAllText("../TalabatApp.Repository/DataSeeding/brands.json");

            var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

            if (brands.Count() > 0)
            {

                brands = brands.Select(P => new ProductBrand()
                {
                    Name = P.Name
                }).ToList();

                if (_context.Brands.Count() == 0)
                {
                    foreach (var brand in brands)
                    {
                        _context.Set<ProductBrand>().Add(brand);
                    }

                    await _context.SaveChangesAsync();
                }
            }
            #endregion


            #region Seeding Categories Data

            var CategoriesData = File.ReadAllText("../TalabatApp.Repository/DataSeeding/categories.json");

            var categories = JsonSerializer.Deserialize<List<ProductCategory>>(CategoriesData);

            if (categories.Count() > 0)
            {

                categories = categories.Select(P => new ProductCategory()
                {
                    Name = P.Name
                }).ToList();

                if (_context.Categories.Count() == 0)
                {
                    foreach (var category in categories)
                    {
                        _context.Set<ProductCategory>().Add(category);
                    }

                    await _context.SaveChangesAsync();
                }
            }
            #endregion

            #region Seeding Products Data

            var ProductsData = File.ReadAllText("../TalabatApp.Repository/DataSeeding/products.json");

            var products = JsonSerializer.Deserialize<List<Product>>(ProductsData);

            if (products.Count() > 0)
            {

                //products = products.Select(P => new Product()
                //{
                //    Name = P.Name
                //}).ToList();

                if (_context.Products.Count() == 0)
                {
                    foreach (var product in products)
                    {
                        _context.Set<Product>().Add(product);
                    }

                    await _context.SaveChangesAsync();
                }
            }
            #endregion


            #region Seeding DeliveryMethods Data

            var deliveryData = File.ReadAllText("../TalabatApp.Repository/DataSeeding/delivery.json");

            var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);

            if (_context.DeliveryMethods.Count() == 0)
            {
                if (deliveryMethods.Count() > 0)
                {
                    foreach (var method in deliveryMethods)
                    {
                        _context.Set<DeliveryMethod>().Add(method);
                    }

                    await _context.SaveChangesAsync();
                }

            }
           

            #endregion
        }
    }
}
