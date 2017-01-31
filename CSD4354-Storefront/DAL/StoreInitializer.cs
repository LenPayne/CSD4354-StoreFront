using CSD4354_Storefront.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CSD4354_Storefront.DAL
{
    public class StoreInitializer : DropCreateDatabaseIfModelChanges<StoreDbContext>
    {
        protected override void Seed(StoreDbContext context)
        {
            var products = new List<Product>
            {
                new Product {Id=1, Name="T-Shirt", Description="Plain White Tees" },
                new Product {Id=2, Name="Sweat Shirt", Description="With a hood!" },
                new Product {Id=3, Name="Under Shirt", Description="With speed holes!" }
            };
            products.ForEach(p => context.Products.Add(p));
            context.SaveChanges();
        }
    }
}