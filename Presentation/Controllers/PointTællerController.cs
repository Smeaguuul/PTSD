using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class PointTællerController : Controller
    {
        public IActionResult Index()
        {
            // Testkamp med forskellige scores
            var kamp = new KampDTO
            {
                KampId = 1,
                Hold1 = "PadelKongerne",
                Hold2 = "NetMasters",
                PointHold1 = "40",
                PointHold2 = "40",
                GamesHold1 = 5,
                GamesHold2 = 6,
                SætHold1 = 1,
                SætHold2 = 1,
                TieBreak = true,
                GoldenBallActive = true
            };

            return View(kamp);
        }
    }
}
