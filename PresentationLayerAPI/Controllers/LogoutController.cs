using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogoutController : Controller
    {
        [HttpGet]
        public IActionResult GetView()
        {
            Response.Cookies.Delete("SessionID");
            Response.Cookies.Delete("Handle");
            Response.Cookies.Delete("Admin");

            return PartialView("LogoutView");
        }
    }
}
