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
    public class ProductCategoryController : ControllerBase
    {
        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductCategory>>> GetProductCategories() =>
            Ok(await _productCategoryService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCategory>> GetProductCategory(string id) =>
            Ok(await _productCategoryService.GetByIdAsync(ObjectId.Parse(id)));

        [HttpPost]
        public async Task<IActionResult> CreateProductCategory([FromBody] ProductCategory productCategcory)
        {
            await _productCategoryService.AddAsync(productCategcory);
            return CreatedAtAction(nameof(GetProductCategory), new { id = productCategcory.Id.ToString() }, productCategcory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductCategory(string id, [FromBody] ProductCategory productCategcory)
        {
            await _productCategoryService.UpdateAsync(ObjectId.Parse(id), productCategcory);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductCategory(string id)
        {
            await _productCategoryService.DeleteAsync(ObjectId.Parse(id));
            return NoContent();
        }

        [HttpPost("join")]
        public async Task<IActionResult> JoinCollections([FromBody] dynamic payload) =>
            Ok(await _productCategoryService.GetJoinedDataAsync(payload));
    }
}
