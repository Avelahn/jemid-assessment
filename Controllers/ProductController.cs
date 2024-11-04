using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;
using ProductAPI.Services;

namespace ProductAPI.Controllers
{
    [Route("api/Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] ProductQueryParams queryParams)
        {
            return await _productService.GetAsync(queryParams);
        }

        // GET: api/Product/PLT001
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            Product? product = await _productService.GetAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // POST: api/Product
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product newProduct)
        {
			// Validate Product attributes
			IDictionary<string, string[]>? validationErrors = newProduct.validate();

			if (validationErrors != null) {
				return ValidationProblem(new ValidationProblemDetails(validationErrors));
			}

            // Store the new product
            await _productService.CreateAsync(newProduct);

            return CreatedAtAction(nameof(GetProduct), new { id = newProduct.Id }, newProduct);
        }

        // PUT: api/Product/PLT001
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> PutProduct(string id, Product updatedProduct)
        {
            Product? product = await _productService.GetAsync(id);

            if (product == null) {
                return NotFound();
            }

            updatedProduct.Id = product.Id;

			// Validate Product attributes
			IDictionary<string, string[]>? validationErrors = updatedProduct.validate();

			if (validationErrors != null) {
				return ValidationProblem(new ValidationProblemDetails(validationErrors));
			}

            await _productService.UpdateAsync(id, updatedProduct);

            return NoContent();
        }

        // DELETE: api/Product/PLT001
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var product = await _productService.GetAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productService.RemoveAsync(id);

            return NoContent();
        }
    }
}
