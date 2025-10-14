using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Newtonsoft.Json;
using PresentationLayerAPI.Models.Request;
using PresentationLayerAPI.Models.Response;
using RestSharp;

namespace PresentationLayerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        [HttpGet("defaultview")]
        public IActionResult GetDefaultView()
        {
            if (Request.Cookies.ContainsKey("SessionID"))
            {
                var handle = Request.Cookies["Handle"];
                var role = Request.Cookies["Admin"];

                if (role == "True")
                {
                    return PartialView("~/Views/Admin/AdminDashboard.cshtml");
                }
                else
                {
                    return PartialView("~/Views/User/UserDefaultView.cshtml");
                }
            }
            return PartialView("LoginDefaultView");
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] LoginRequestUser user)
        {
            RestClient client = new RestClient("http://localhost:5295");
            RestRequest request = new RestRequest("/api/LoginWithHandle", Method.Post);
            request.AddJsonBody(user);
            RestResponse response = client.Execute(request);
            LoginResponseUser loginResponse = JsonConvert.DeserializeObject<LoginResponseUser>(response.Content);

            if (loginResponse.Success == false)
            {
                return PartialView("LoginErrorView");
            }
            else if (loginResponse.Success == true)
            {
                Response.Cookies.Append("SessionID", Guid.NewGuid().ToString());
                Response.Cookies.Append("Handle", user.Handle);
                Response.Cookies.Append("Admin", loginResponse.Admin.ToString());
                if (loginResponse.Admin == true)
                {
                    return PartialView("~/Views/Admin/AdminDashboard.cshtml");
                }
                else
                {
                    return PartialView("~/Views/User/UserDefaultView.cshtml");
                }               
            }
            return PartialView("LoginErrorView");
        }
    }
}
