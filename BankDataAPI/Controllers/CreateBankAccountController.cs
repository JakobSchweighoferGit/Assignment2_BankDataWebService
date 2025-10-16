using BankDataAPI.Data;
using BusinessLayerAPI.Data;
using BusinessLayerAPI.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusinessLayerAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CreateBankAccountController : ControllerBase
    {
        [HttpPost("createBankAccount")]
        public IActionResult Create([FromBody] CreateBankAccountRequest req)
        {
            if (req == null || string.IsNullOrWhiteSpace(req.AccountNumber) || string.IsNullOrWhiteSpace(req.Handle))
            {
                return BadRequest(new { success = false, message = "AccountNumber and Handle are required" });
            }

            var user = UserProfileManagement.DataGetUserByHandle(req.Handle);

            if (user == null)
            {
                return BadRequest(new { success = false, message = "User (handle) not found" });
            }

            var newId = AccountManagement.InsertAccount(req.AccountNumber, req.Balance, user.UserID, req.Active);

            if (newId == null)
            {
                return StatusCode(500, new { success = false, message = "Insert failed" });
            }

            return Ok(new { Success = true, message = "Account created" });
        }
    }
}
