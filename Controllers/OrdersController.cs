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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders() =>
            Ok(await _orderService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(string id) =>
            Ok(await _orderService.GetByIdAsync(ObjectId.Parse(id)));

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            await _orderService.AddAsync(order);
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id.ToString() }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(string id, [FromBody] Order order)
        {
            await _orderService.UpdateAsync(ObjectId.Parse(id), order);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            await _orderService.DeleteAsync(ObjectId.Parse(id));
            return NoContent();
        }

        [HttpPost("join")]
        public async Task<IActionResult> JoinCollections([FromBody] dynamic payload) =>
            Ok(await _orderService.GetJoinedDataAsync(payload));
    }
}
