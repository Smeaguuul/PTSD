using System.Diagnostics;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly MatchesService matchesService;
        private readonly GiveawayService giveawayService;
        public HomeController(MatchesService matchesService, GiveawayService giveawayService)
        {
            this.matchesService = matchesService;
            this.giveawayService = giveawayService;
        }

        public async Task<IActionResult> Index()
        {
            var ongoingMatches = await matchesService.OngoingMatches();

            if (ongoingMatches.ToList().Count == 0)
            {
                await matchesService.SeedMatchData();
                await giveawayService.SeedData();
                
                ongoingMatches = await matchesService.OngoingMatches();
            }

            Matches matches = new Matches();
            foreach (var match in ongoingMatches)
            {
                var matchScore = await matchesService.GetMatchScore(match.Id);
                matches.MatchScores.Add(matchScore);
            }

            return View(matches);
        }
    }
}
