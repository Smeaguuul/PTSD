using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class PointTællerController : Controller
    {
        public IActionResult Overblik()
        {
            // Testliste med tre kampe
            var kampe = new List<KampDTO>
    {
        new KampDTO
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
        },
        new KampDTO
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
        },
        new KampDTO
        {
            KampId = 3,
            Hold1 = "AceBreakers",
            Hold2 = "Smashers",
            PointHold1 = "40",
            PointHold2 = "40",
            GamesHold1 = 3,
            GamesHold2 = 3,
            SætHold1 = 1,
            SætHold2 = 2,
            TieBreak = true,
            GoldenBallActive = false
        }
    };

            return View(kampe);
        }
    }
}