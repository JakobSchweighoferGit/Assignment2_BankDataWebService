using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PresentationLayerAPI.Models.Request;
using PresentationLayerAPI.Models.Response;
using RestSharp;

namespace PresentationLayerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminUserManagementController : Controller
    {
        [HttpPost("adminUserManagmentListInformation")]
        public IActionResult GetUserManagment([FromBody] SearchUserRequest data)
        {
            try
            {
                if (data.SearchString == null)
                {
                    data.SearchString = "";
                }

                var client = new RestClient("http://localhost:5295");
                var request = new RestRequest("/api/search", Method.Post);
                request.AddJsonBody(data);
                RestResponse response = client.Execute(request);
                List<UserSearchInformation> userList = JsonConvert.DeserializeObject<List<UserSearchInformation>>(response.Content);

                return PartialView("~/Views/Admin/AdminUserManagement.cshtml", userList);
            }
            catch
            {
                return PartialView("~/Views/Login/LoginErrorView.cshtml");
            }
        }


        [HttpPost("adminUserEditInformation")]
        public IActionResult GetAdmininformation([FromBody] GetUserInformationRequest data)
        {
            try
            {

                RestClient client = new RestClient("http://localhost:5295");
                RestRequest request = new RestRequest("/api/GetUserinformationByHandle", Method.Get);
                request.AddJsonBody(data);
                RestResponse response = client.Execute(request);
                GetUserInformationResponse userInformation = JsonConvert.DeserializeObject<GetUserInformationResponse>(response.Content);
                return PartialView("~/Views/Admin/AdminEditUserInformation.cshtml", userInformation);

            }
            catch (Exception ex)
            {
                return PartialView("~/Views/Login/LoginErrorView.cshtml");
            }
        }

        [HttpPost("adminEditUser")]
        public IActionResult EditUser([FromBody] EditUserRequest data)
        {
            try
            {
                var client = new RestClient("http://localhost:5295");
                var request = new RestRequest("/api/EditUser", Method.Post);
                request.AddJsonBody(data);
                RestResponse response = client.Execute(request);
                SucessResponse editResponse = JsonConvert.DeserializeObject<SucessResponse>(response.Content);

                if (!editResponse.Success)
                {
                    return BadRequest(new { success = false, message = "Update failed" });
                }

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}
