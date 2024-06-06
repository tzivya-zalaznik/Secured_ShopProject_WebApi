using Entities;

namespace Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetALlProducts(float? minPrice, float? maxPrice, int?[] category, string? description);
        Task<Product> GetProductById(int id);
    }
}