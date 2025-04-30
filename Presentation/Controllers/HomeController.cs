using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            Matches model = new(["1-1", "1-0", "0-0"]);

            return View(model);
        }
    }
}
