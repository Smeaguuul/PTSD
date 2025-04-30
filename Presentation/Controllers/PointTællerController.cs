using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class PointTællerController : Controller
    {
        public IActionResult Overblik()
        {
            // Testliste med to kampe
            var kampe = new List<KampDTO>
            {
                new KampDTO { KampId = 1, Hold1 = "PadelKongerne", Hold2 = "NetMasters" },
                new KampDTO { KampId = 2, Hold1 = "Team Smash", Hold2 = "VolleyBeasts" }
            };

            return View(kampe);
        }

        public IActionResult Index(int id)
        {
            // Dummy switch – her ville du normalt slå op i databasen
            KampDTO kamp;

            if (id == 1)
            {
                kamp = new KampDTO
                {
                    KampId = 1,
                    Hold1 = "PadelKongerne",
                    Hold2 = "NetMasters",
                    PointHold1 = "30",
                    PointHold2 = "40",
                    GamesHold1 = 4,
                    GamesHold2 = 5,
                    SætHold1 = 1,
                    SætHold2 = 1,
                    TieBreak = false,
                    GoldenBallActive = true
                };
            }
            else
            {
                kamp = new KampDTO
                {
                    KampId = 2,
                    Hold1 = "Team Smash",
                    Hold2 = "VolleyBeasts",
                    PointHold1 = "15",
                    PointHold2 = "15",
                    GamesHold1 = 2,
                    GamesHold2 = 2,
                    SætHold1 = 0,
                    SætHold2 = 0,
                    TieBreak = false,
                    GoldenBallActive = false
                };
            }

            return View(kamp);
        }
    }
}
