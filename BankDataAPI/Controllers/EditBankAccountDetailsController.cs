using BankDataAPI.Data;
using BusinessLayerAPI.Data;
using RequestsResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusinessLayerAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class EditBankAccountDetailsController : ControllerBase
    {
        [HttpPost("EditBankAccount")]
        public ActionResult EditBankAccount([FromBody] EditBankAccountRequest req)
        {
            var user = UserProfileManagement.DataGetUserByHandle(req.Handle);
            if (user == null)
            {
                return BadRequest(new { success = false, message = "Handle does not exist" });
            }

            // Perform update
            var ok = AccountManagement.UpdateAccount(req);

            if (!ok)
            {
                return StatusCode(500, new { success = false, message = "Update failed" });
            }

            return Ok(new { success = true });
        }
    }
}
