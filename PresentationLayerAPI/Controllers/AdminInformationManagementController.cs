using System.Reflection.Metadata;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PresentationLayerAPI.Models.Request;
using PresentationLayerAPI.Models.Response;
using RequestsResponses;

using RestSharp;

namespace PresentationLayerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminInformationManagementController : Controller
    {
        [HttpGet("adminInformation")]
        public IActionResult GetAdmininformation()
        {
            try
            {
                var handle = Request.Cookies["Handle"];

                RestClient client = new RestClient("http://localhost:5295");
                RestRequest request = new RestRequest("/api/GetUserinformationByHandle", Method.Get);
                request.AddJsonBody(new GetUserInformationRequestHandle { Handle = handle });
                RestResponse response = client.Execute(request);
                GetUserInformationResponse userInformation = JsonConvert.DeserializeObject<GetUserInformationResponse>(response.Content);
                return PartialView("~/Views/Admin/AdminInformation.cshtml", userInformation);

            }
            catch (Exception ex)
            {
                return PartialView("~/Views/Login/LoginErrorView.cshtml");
            }
        }

        [HttpPost("editAdmin")]
        public IActionResult EditUser([FromBody] EditUserRequest payload)
        {
            try
            {
                if (!Request.Cookies.ContainsKey("SessionID"))
                    return Unauthorized(new { success = false, message = "Not logged in" });

                var client = new RestClient("http://localhost:5295");
                var request = new RestRequest("/api/EditUser", Method.Post);
                request.AddJsonBody(payload);
                RestResponse response = client.Execute(request);
                SucessResponse userInformation = JsonConvert.DeserializeObject<SucessResponse>(response.Content);

                if (!userInformation.Success)
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
