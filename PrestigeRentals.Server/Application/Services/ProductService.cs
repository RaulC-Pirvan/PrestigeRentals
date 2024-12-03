using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrestigeRentals.Application.Interfaces;
using PrestigeRentals.Domain.Entities;

namespace PrestigeRentals.Application.Services
{
    public class ProductService : IProductService
    {
        public IEnumerable<Product> GetAllProducts()
        {
            return new List<Product>
            {
                new Product {Id = 1, Name = "Product1", Price = 100},
                new Product {Id = 2, Name = "Product2", Price = 200},
                new Product {Id = 3, Name = "Product3", Price = 300}
            };
        }
    }
}
