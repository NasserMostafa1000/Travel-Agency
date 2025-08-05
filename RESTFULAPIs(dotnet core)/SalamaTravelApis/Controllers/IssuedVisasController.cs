using Microsoft.AspNetCore.Mvc;
using SalamaTravelDTA;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalamaTravelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssuedVisaController : ControllerBase
    {
        [HttpGet("GetIssuedVisas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetIssuedVisas( string Token, int OwnerId)
        {
            try
            {
                if (string.IsNullOrEmpty(Token) || OwnerId <= 0)
                    return BadRequest("Invalid token or owner ID.");

                var visas = await  IssuedVisaDAL.GetIssuedVisasAsync(Token, OwnerId);

                if (visas == null || visas.Count == 0)
                    return NotFound("No issued visas found.");

                return Ok(visas);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // تسجيل الأخطاء أو إرسالها للخادم لتحليلها
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
