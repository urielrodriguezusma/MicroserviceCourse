using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly ILogger<CatalogController> logger;

        public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger)
        {
            this.productRepository = productRepository;
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<Product>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts()
        {
            var products = await this.productRepository.GetProducts().ConfigureAwait(false);
            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            var product = await this.productRepository.GetProduct(id).ConfigureAwait(false);
            if (product == null)
            {
                this.logger.LogError($"Product with id: {id}, not found.");
                return NotFound();
            }
            return Ok(product);
        }

        [Route("[action]/{category}", Name = "GetProductByCategory")]
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<Product>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProductByCategory(string category)
        {
            var products = await this.productRepository.GetProductByCategory(category).ConfigureAwait(false);
            return Ok(products);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await this.productRepository.CreateProduct(product).ConfigureAwait(false);
            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            var isProductUpdated = await this.productRepository.UpdateProduct(product).ConfigureAwait(false);
            return Ok(isProductUpdated);
        }

        [HttpDelete("{id:length(24)}",Name ="DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var isProductDelete = await this.productRepository.DeleteProduct(id).ConfigureAwait(false);
            return Ok(isProductDelete);
        }

    }
}
