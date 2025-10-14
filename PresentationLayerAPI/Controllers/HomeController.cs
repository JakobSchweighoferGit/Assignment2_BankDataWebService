using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PresentationLayerAPI.Models;
//using SimpleWebJS.Models;

namespace PresentationLayerAPI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return PartialView();
        }
    }
}
