using BusinessLayerAPI.Data;
using RequestsResponses;
using Microsoft.AspNetCore.Mvc;

namespace BusinessLayerAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UpdateBalanceController : ControllerBase
    {
        [HttpPost("updateBalance")]
        public IActionResult updateBalance(CreateBalanceUpdateRequest req)
        {
            Console.WriteLine($"Updating user {req.AccountID}'s balance");
            if (req is null)
            {
                return BadRequest(new { success = false, message = "Handle is required" });
            }

            try
            {
                var ok = TransactionManagement.updateBalance(req);

                if (!ok)
                {
                    return StatusCode(400, new { success = false, message = $"Balance update failed for Account ID {req.AccountID}. Account may not exist or database error occurred." });
                }

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Controller error during balance update: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Internal server error during balance update." });
            }
        }
    }
}
