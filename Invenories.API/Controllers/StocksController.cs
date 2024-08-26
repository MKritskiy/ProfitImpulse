
using Inventories.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Inventories.API.Controllers
{
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StocksController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet("inventories/{profileid}")]
        public async Task<IActionResult> Index(int profileid)
        {
            var jwtToken = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(jwtToken))
            {
                return Unauthorized("Missing or invalid token.");
            }

            var stocks = await _stockService.GetStocksAsync(profileid, jwtToken);

            if (stocks == null || !stocks.Any())
            {
                return NotFound("Stocks not found.");
            }

            return Ok(stocks);
        }
    }
}
