using BankDataAPI.Data;
using BusinessLayerAPI.Models.Request;
using BusinessLayerAPI.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankDataAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class GetUserinformationByHandleController : ControllerBase
    {

        [HttpGet("GetUserinformationByHandle")]
        public ActionResult<UserDetailsResponse> GetUserInformationByHandle([FromBody] GetUserInformationRequestHandle request)
        {
            try
            {
                var foundUser = UserProfileManagement.DataGetUserByHandle(request.Handle);
                return Ok(new UserDetailsResponse
                {
                    Handle = foundUser.Handle,
                    UserID = foundUser.UserID,
                    Address = foundUser.Address,
                    FirstName = foundUser.FirstName,    
                    LastName = foundUser.LastName,  
                    Email = foundUser.Email,
                    Password = foundUser.Password,
                    Phone = foundUser.Phone,
                    PicturePath = foundUser.PicturePath,
                    Admin = foundUser.Admin,
                });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}   
