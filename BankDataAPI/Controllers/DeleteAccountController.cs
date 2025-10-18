using BankDataAPI.Data;
using BusinessLayerAPI.Data;
using RequestsResponses;
using Microsoft.AspNetCore.Mvc;

namespace BusinessLayerAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class DeleteAccountController : ControllerBase
    {

        [HttpPost("deleteAccount")]
        public IActionResult DeleteAccount([FromBody] DeleteAccountRequest req)
        {
            if (req == null ||
                string.IsNullOrWhiteSpace(req.AccountNumber))
            {
                return BadRequest(new { success = false, message = "Missing required fields" });
            }

           
            var ok = AccountManagement.DeleteAccount(req.AccountNumber);
            if (!ok)
            {
                return StatusCode(500, new { success = false, message = "Delete failed" });
            }

            return Ok(new { success = true });
        }
    }
}
