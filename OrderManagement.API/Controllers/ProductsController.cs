using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;

namespace OrderManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
            private readonly IProductService _productService;

            public ProductsController(IProductService productService)
            {
                _productService = productService;
            }

            
            [HttpGet] // gets all products (GET: api/products)
        public async Task<IActionResult> GetAll()
            {
                var products = await _productService.GetProductsAsync();
                return Ok(products); // 200 OK yanıtı ve ürün listesini döner.
            }

             
            [HttpPost] //adds new product (POST: api/products)
        public async Task<IActionResult> Create(Product product)
            {
                await _productService.CreateProductAsync(product);
                // Ürün oluşunca, onu nereden bulabileceğimizi (GetAll) ve ürünü döner.
                return CreatedAtAction(nameof(GetAll), new { id = product.Id }, product);
            }

        [HttpPut("{id}/add-stock")]
        public async Task<IActionResult> AddStock(int id, [FromBody] int quantity)
        {
            var result = await _productService.AddStockAsync(id, quantity);
            if (!result) return BadRequest("Stock not updated");
            return Ok("Stock successfully updated.");
        }
    }
    }

