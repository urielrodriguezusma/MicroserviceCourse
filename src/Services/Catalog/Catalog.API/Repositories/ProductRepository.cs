using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext catalogContext;

        public ProductRepository(ICatalogContext catalogContext)
        {
            this.catalogContext = catalogContext;
        }
        public async Task CreateProduct(Product product)
        {
            await this.catalogContext.Products.InsertOneAsync(product);
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateProduct = await this.catalogContext
                                          .Products
                                          .ReplaceOneAsync(filter: g => g.Id.Equals(product.Id), replacement: product);

            return updateProduct.IsAcknowledged && updateProduct.ModifiedCount > 0;
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(d => d.Id, id);
            var deletedProduct = await this.catalogContext.Products.DeleteOneAsync(filter);
            return deletedProduct.IsAcknowledged && deletedProduct.DeletedCount > 0;
        }

        public async Task<Product> GetProduct(string id)
        {
            return await this.catalogContext.Products.Find(d => d.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<Product>> GetProductByCategory(string categoryName)
        {
            FilterDefinition<Product> filterCategory = Builders<Product>.Filter.Eq(d => d.Category, categoryName);
            return await this.catalogContext.Products.Find(filterCategory).ToListAsync();
        }

        public async Task<IReadOnlyList<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);
            return await this.catalogContext.Products.Find(filter).ToListAsync();
        }

        public async Task<IReadOnlyList<Product>> GetProducts()
        {
            return await this.catalogContext.Products.Find(p => true).ToListAsync();
        }
    }
}
