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
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers() =>
            Ok(await _supplierService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<Supplier>> GetSupplier(string id) =>
            Ok(await _supplierService.GetByIdAsync(ObjectId.Parse(id)));

        [HttpPost]
        public async Task<IActionResult> CreateSupplier([FromBody] Supplier supplier)
        {
            await _supplierService.AddAsync(supplier);
            return CreatedAtAction(nameof(GetSupplier), new { id = supplier.Id.ToString() }, supplier);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(string id, [FromBody] Supplier supplier)
        {
            await _supplierService.UpdateAsync(ObjectId.Parse(id), supplier);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(string id)
        {
            await _supplierService.DeleteAsync(ObjectId.Parse(id));
            return NoContent();
        }

        [HttpPost("join")]
        public async Task<IActionResult> JoinCollections([FromBody] dynamic payload) =>
            Ok(await _supplierService.GetJoinedDataAsync(payload));
    }
}
