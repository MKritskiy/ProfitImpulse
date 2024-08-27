
using Purchases.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Purchases.API.Controllers
{
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly IPurchaseService _PurchaseService;

        public PurchasesController(IPurchaseService PurchaseService)
        {
            _PurchaseService = PurchaseService;
        }

        [HttpGet("purchases/{profileid}")]
        public async Task<IActionResult> Index(int profileid)
        {
            try
            {

                var jwtToken = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(jwtToken))
                {
                    return Unauthorized("Missing or invalid token.");
                }

                var Purchases = await _PurchaseService.GetPurchasesAsync(profileid, jwtToken);

                if (Purchases == null || !Purchases.Any())
                {
                    return NotFound("Purchases not found.");
                }

                return Ok(Purchases);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
