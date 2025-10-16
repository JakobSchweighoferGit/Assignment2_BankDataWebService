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
    public class AdminCreateUserController : Controller
    {
        [HttpGet("createUserView")]
        public IActionResult GetDefaultView()
        {
            return PartialView("~/Views/Admin/AdminCreateUserView.cshtml");
        }

        [HttpPost("admincreatesUserWithData")]
        public IActionResult EditUser([FromBody] CreateUserRequest data)
        {
            try
            {
                var client = new RestClient("http://localhost:5295");
                var request = new RestRequest("/api/createUser", Method.Post);
                request.AddJsonBody(data);
                RestResponse response = client.Execute(request);
                SucessResponse createResponse = JsonConvert.DeserializeObject<SucessResponse>(response.Content);

                if (createResponse == null)
                {
                    return BadRequest(new { success = false, message = "No Server Response" });
                }

                if (!createResponse.Success)
                {
                    return BadRequest(new { success = false, message = createResponse.Message });
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
