using BankDataAPI.Data;
using BusinessLayerAPI.Data;
using Microsoft.AspNetCore.Mvc;
using RequestsResponses;

namespace BusinessLayerAPI.Controllers
{

    [Route("api/")]
    [ApiController]
    public class CreateTransactionController : ControllerBase
    {
        [HttpPost("createTransaction")]
        public IActionResult CreateTransaction([FromBody] CreateTransactionRequest req)
        {
            Console.WriteLine("Adding a new transaction");
            if (req is null)
            {
                return BadRequest(new { success = false, message = "Handle is required" });
            }

            try
            {
                var ok = TransactionManagement.createTransaction(req);

                if (!ok)
                {
                    return StatusCode(500, new { success = false, message = "Transaction creation failed due to database error." });
                }

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Controller error during transaction creation: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Internal server error during transaction creation." });
            }
        }

    }
}
