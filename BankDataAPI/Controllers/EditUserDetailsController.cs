using BankDataAPI.Data;
using BusinessLayerAPI.Models.Request;
using BusinessLayerAPI.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusinessLayerAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class EditUserDetailsController : ControllerBase
    {
        [HttpPost("EditUser")]
        public ActionResult EditUser([FromBody] EditUserRequest req)
        {
            Console.WriteLine("Wir san in edit drinna");
            if (req is null)
            {
                return BadRequest(new { success = false, message = "Handle is required" });
            }
                
            var ok = UserProfileManagement.EditUserInformation(req);
            Console.WriteLine("Wir kennan scho amol schreiba");
            if (!ok)
            {
                return StatusCode(500, new { success = false, message = "Update failed" });
            }
                
            return Ok(new { success = true });
        }
    }
}