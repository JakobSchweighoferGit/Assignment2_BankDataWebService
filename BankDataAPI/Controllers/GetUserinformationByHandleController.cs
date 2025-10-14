using BankDataAPI.Data;
using BusinessLayerAPI.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankDataAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class GetUserinformationByHandleController : ControllerBase
    {

        [HttpGet("GetUserinformationByHandle/{handle}")]
        public ActionResult<UserDetailsResponse> GetUserInformationByHandle(string handle)
        {
            try
            {
                var user = UserProfileManagment.DataGetUserByHandle(handle);
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}   
