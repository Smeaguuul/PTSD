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
            int[] fieldIds = { 1, 2, 3 };
            ViewBag.FieldIds = fieldIds;
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
