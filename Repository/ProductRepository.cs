using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ProductRepository : IProductRepository
    {
        private AdoNetUsers214956807Context _productsContext;
        public ProductRepository(AdoNetUsers214956807Context productsContext)
        {
            _productsContext = productsContext;
        }
        public async Task<List<Product>> GetALlProducts(float? minPrice, float? maxPrice, int?[] category, string? description)
        {
           var query = _productsContext.Products.Where(product =>
           (description == null ? (true) : (product.Description.Contains(description)))
           && (minPrice == null ? (true) : (product.Price >= minPrice))
           && (maxPrice == null ? (true) : (product.Price <= maxPrice))
           && (category.Length == 0 ? (true) : (category.Contains(product.CategoryId)))).OrderBy(p => p.Price).Include(i => i.Category);

            Console.WriteLine(query.ToQueryString());
            List<Product> products = await query.ToListAsync();
            return products;
        }
        public async Task<Product> GetProductById(int id)
        {
            return await _productsContext.Products.FindAsync(id);
        }
    }
}
