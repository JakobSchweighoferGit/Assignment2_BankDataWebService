using BankDataAPI.Data;
using BusinessLayerAPI.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace BusinessLayerAPI.Controllers
{

    [Route("api/")]
    [ApiController]
    public class DeleteUserController : ControllerBase
    {
        [HttpPost("deleteUserByEmail")]
        public IActionResult DeleteUserByEmail([FromBody] DeleteUserRequest req)
        {
            if (req == null ||
                string.IsNullOrWhiteSpace(req.Email))
            {
                return BadRequest(new { success = false, message = "Missing required fields" });
            }

            var existingUser = UserProfileManagement.DataGetUserByHandle(req.Email);
            if (existingUser == null)
            {
                return Conflict(new { success = false, message = "User does not exist" });
            }

            var ok = UserProfileManagement.DeleteUserByEmail(req.Email);
            if (!ok)
            {
                return StatusCode(500, new { success = false, message = "Delete failed" });
            }

            return Ok(new { success = true });
        }

        [HttpPost("deleteUserByHandle")]
        public IActionResult DeleteUserByHandle([FromBody] DeleteUserRequest req)
        {
            if (req == null ||
                string.IsNullOrWhiteSpace(req.Handle))
            {
                return BadRequest(new { success = false, message = "Missing required fields" });
            }

            var existingUser = UserProfileManagement.DataGetUserByHandle(req.Handle);
            if (existingUser == null)
            {
                return Conflict(new { success = false, message = "User does not exist" });
            }

            var ok = UserProfileManagement.DeleteUserByHandle(req.Email);
            if (!ok)
            {
                return StatusCode(500, new { success = false, message = "Delete failed" });
            }

            return Ok(new { success = true });
        }
    }
}
