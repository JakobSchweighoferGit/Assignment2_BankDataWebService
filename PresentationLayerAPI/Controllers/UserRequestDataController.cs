using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PresentationLayerAPI.Models.Request;
using RestSharp;
using Microsoft.AspNetCore.Http;
using RequestsResponses;


//get user information by handle
// let js code parse in the web side

namespace PresentationLayerAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserRequestDataController : ControllerBase 
    {
        public IActionResult GetUserDetails([FromBody] GetUserInformationRequestHandle req)
        {
            try
            {
                if (req.Handle == null)
                {
                    return BadRequest("Request Object cannot be null");
                }

                var client = new RestClient("http://localhost:5295");
                var request = new RestRequest("/api/GetUserinformationByHandle", Method.Post);
                request.AddJsonBody(req);
                RestResponse response = client.Execute(request);

                GetUserInformationResponse userDetails =
                    JsonConvert.DeserializeObject<GetUserInformationResponse>(response.Content);

                return Ok(userDetails);

            }
            catch
            {
                
                return StatusCode(500, "An error occurred while fetching user details.");
            }
        }
    } 
}
