using System.Diagnostics;
using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMatchesService matchesService;
        private readonly IGiveawayService giveawayService;
        public HomeController(IMatchesService matchesService, IGiveawayService giveawayService)
        {
            this.matchesService = matchesService;
            this.giveawayService = giveawayService;
        }

        public async Task<IActionResult> Index()
        {
            var ongoingMatches = await matchesService.OngoingMatches();

            Matches matches = new Matches();
            foreach (var match in ongoingMatches)
            {
                var matchScore = await matchesService.GetMatchScore(match.Id);
                matches.MatchScores.Add(matchScore);
            }

            var matchesToday = await matchesService.GetTodaysMatchesWithScore();
            ViewBag.homeWins = 0;
            ViewBag.awayWins = 0;
            foreach (var match in matchesToday)
            {
                if (match.Score.Sets.Count(s => s.Winner == true) == 2)
                {
                    ViewBag.homeWins++;
                }
                if (match.Score.Sets.Count(s => s.Winner == false) == 2)
                {
                    ViewBag.awayWins++;
                }
            }

            // ?? Tilføjelse: hent reklamebilleder fra /wwwroot/images/Ads
            var adDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Ads");
            var adImages = Directory.GetFiles(adDir)
                .Select(Path.GetFileName)
                .Where(f => f.EndsWith(".jpg") || f.EndsWith(".png"))
                .ToList();

            ViewBag.AdImages = adImages;

            return View(matches);
        }
    }
}

