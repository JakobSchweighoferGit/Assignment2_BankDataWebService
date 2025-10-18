using BusinessLayerAPI.Data;
using RequestsResponses;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusinessLayerAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class GetBankAccountInformationByAccountIDController : ControllerBase
    {
        [HttpPost("GetBankAccountInformationById")]
        public ActionResult<AccountDetailsResponse> GetBankAccountInformationById([FromBody] GetBankAccountinformationRequest req)
        {
            if (req == null || req.AccountID <= 0)
                return BadRequest(new { error = "Valid AccountID is required" });

            var acc = AccountManagement.GetAccountById(req.AccountID);
            if (acc == null)
                return NotFound(new { error = "Account not found" });

            var resp = new AccountDetailsResponse
            {
                AccountID = acc.Value.AccountID,
                AccountNumber = acc.Value.AccountNumber,
                Balance = acc.Value.Balance,
                Active = acc.Value.Active,
                UserID = acc.Value.UserID,
                Handle = acc.Value.Handle
            };

            return Ok(resp);
        }
    }
}
