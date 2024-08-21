using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MultipleJoins.Interfaces.Services;
using MultipleJoins.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultipleJoins.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetOrders() =>
            Ok(await _productService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(string id) =>
            Ok(await _productService.GetByIdAsync(ObjectId.Parse(id)));

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            await _productService.AddAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id.ToString() }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatProduct(string id, [FromBody] Product product)
        {
            await _productService.UpdateAsync(ObjectId.Parse(id), product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            await _productService.DeleteAsync(ObjectId.Parse(id));
            return NoContent();
        }

        [HttpPost("join")]
        public async Task<IActionResult> JoinCollections([FromBody] dynamic payload) =>
            Ok(await _productService.GetJoinedDataAsync(payload));
    }
}
