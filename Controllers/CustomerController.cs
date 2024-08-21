using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MultipleJoins.Implementations.Services;
using MultipleJoins.Interfaces.Services;
using MultipleJoins.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultipleJoins.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetOrders() =>
            Ok(await _customerService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id) =>
            Ok(await _customerService.GetByIdAsync(ObjectId.Parse(id)));

        [HttpPost]
        public async Task<IActionResult> AddCustomer(Customer customer)
        {
            await _customerService.AddAsync(customer);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id.ToString() }, customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(string id, Customer customer)
        {
            await _customerService.UpdateAsync(ObjectId.Parse(id), customer);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            await _customerService.DeleteAsync(ObjectId.Parse(id));
            return NoContent();
        }

        [HttpPost("join")]
        public async Task<IActionResult> GetJoinedData(dynamic payload)
        {
            var result = await _customerService.GetJoinedDataAsync(payload);
            return Ok(result);
        }
    }
}
