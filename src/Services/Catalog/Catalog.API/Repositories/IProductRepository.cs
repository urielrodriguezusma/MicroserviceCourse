using Catalog.API.Entities;

namespace Catalog.API.Repositories
{
    public interface IProductRepository
    {
        Task<IReadOnlyList<Product>> GetProducts();
        
        Task<Product> GetProduct(string id);
        Task<IReadOnlyList<Product>> GetProductByName(string name);
        Task<IReadOnlyList<Product>> GetProductByCategory(string categoryName);
        Task CreateProduct(Product product);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(string id);
    }
}
