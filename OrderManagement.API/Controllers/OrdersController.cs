using Microsoft.AspNetCore.Mvc;
using OrderManagement.Core.Interfaces;
using OrderManagement.Core.Entities;

namespace OrderManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            orderService = orderService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            var result = await _orderService.CreateOrderAsync(order);
            if (!result)
            {
                return BadRequest("Failed to create order. Stock is insufficient or data is invalid.");
            }

            return Ok(new { message = "Order successfully created and stocks are updated.",orderId=order.Id });
            
        }
    }
}
