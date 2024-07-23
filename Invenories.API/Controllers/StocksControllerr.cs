
using Inventories.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Inventories.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StocksControllerr : ControllerBase
    {
        private readonly IStockService _stockService;

        public StocksControllerr(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet("{profileid}")]
        public async Task<IActionResult> Index(int profileid)
        {
            var stocks = await _stockService.GetStocksAsync(profileid);
            return Ok(stocks);
        }
    }
}
