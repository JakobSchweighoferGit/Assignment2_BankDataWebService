using BankDataAPI.Data;
using BusinessLayerAPI.Models;
using RequestsResponses;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusinessLayerAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class GetUserInformationBySearchStringController : ControllerBase
    {
        [HttpPost("search")]
        public ActionResult<List<UserSearchInformation>> SearchUsers([FromBody] GetUserInformationSearchStringRequest request)
        {
            var users = UserProfileManagement.GetUsersBySearch(request?.SearchString);
            return Ok(users);
        }
    }
}
