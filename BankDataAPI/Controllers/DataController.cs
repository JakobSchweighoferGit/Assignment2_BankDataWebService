using BankDataAPI.Data;
using BankDataAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankDataAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class DataController : ControllerBase
    {

        [HttpGet("GetUserinformationByHandle/{handle}")]
        public ActionResult<UserDetails> GetUserInformationByHandle(string handle)
        {
            try
            {
                var user = DBManager.DataGetUserByHandle(handle);
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet("LoginWithHandle/{handle}/{password}")]
        public ActionResult<UserDetails> LoginWithHandle(string handle, string password)
        {
            try
            {
                var user = DBManager.DataGetUserByHandle(handle);

                if (user == null)
                {
                    return NotFound(new { error = "User not found" });
                }
                Console.WriteLine("Passwort ist: " + user.Password);

                if (user.Password != "test123")
                {
                    return Unauthorized(new { error = "Incorrect password" });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}   
