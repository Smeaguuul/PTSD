using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class AdminController : Controller
    {
        public AdminController()
        {
            
        }
        public ActionResult Admin()
        {
            return View();
        }
        public ActionResult StartGame()
        {
            return View();
        }
        public ActionResult StartGameForm() {

            return RedirectToAction("Admin");
        }
    }
}
