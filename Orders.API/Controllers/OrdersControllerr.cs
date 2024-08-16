
using Orders.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Inventories.API.Controllers
{
    [ApiController]
    public class OrdersControllerr : ControllerBase
    {
        private readonly IOrderService _OrderService;

        public OrdersControllerr(IOrderService OrderService)
        {
            _OrderService = OrderService;
        }

        [HttpGet("{profileid}")]
        public async Task<IActionResult> Index(int profileid)
        {
            var jwtToken = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(jwtToken))
            {
                return Unauthorized("Missing or invalid token.");
            }

            var Orders = await _OrderService.GetOrdersAsync(profileid, jwtToken);

            if (Orders == null || !Orders.Any())
            {
                return NotFound("Orders not found.");
            }

            return Ok(Orders);
        }
    }
}
