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
