using System.Reflection.Metadata;
using BankDataAPI.Data;
using BusinessLayerAPI.Models.Request;
using BusinessLayerAPI.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankDataAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost("LoginWithHandle")]
        public ActionResult<UserLoginDataResponse> LoginWithHandle([FromBody] UserLoginRequest user)
        {
            try
            {
                var foundUser = UserProfileManagment.DataGetUserByHandle(user.Handle);
                //Console.WriteLine("Wir haben den User: " + foundUser.Handle);
                if (foundUser == null)
                    return Unauthorized(new UserLoginDataResponse {Handle = "", Admin = false, Success = false, Message = "User not found" });

                if (foundUser.Password != user.PassWord)
                    return Unauthorized(new UserLoginDataResponse { Handle = "", Admin = false, Success = false, Message = "Wrong password" });

                return Ok(new UserLoginDataResponse
                {
                    Handle = foundUser.Handle,
                    Admin = foundUser.Admin,
                    Success = true,
                    Message = "Login successful"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new UserLoginDataResponse { Success = false, Message = ex.Message });
            }
        }
    }
}
