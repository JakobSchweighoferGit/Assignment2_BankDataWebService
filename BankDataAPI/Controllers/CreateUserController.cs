using BankDataAPI.Data;
using RequestsResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusinessLayerAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CreateUserController : ControllerBase
    {
        [HttpPost("createUser")]
        public IActionResult Create([FromBody] CreateUserRequest req)
        {
            if (req == null ||
                string.IsNullOrWhiteSpace(req.Handle) ||
                string.IsNullOrWhiteSpace(req.FirstName) ||
                string.IsNullOrWhiteSpace(req.LastName) ||
                string.IsNullOrWhiteSpace(req.Email) ||
                string.IsNullOrWhiteSpace(req.Password))
            {
                return BadRequest(new { success = false, message = "Missing required fields" });
            }

            var existingUser = UserProfileManagement.DataGetUserByHandle(req.Handle);
            if (existingUser != null)
            {
                return Conflict(new { success = false, message = "Handle already exists" });
            }

            var ok = UserProfileManagement.InsertUser(req);
            if (!ok)
            {
                return StatusCode(500, new { success = false, message = "Insert failed" });
            }

            return Ok(new { success = true });
        }
    }
}
