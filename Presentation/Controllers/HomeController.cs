using System.Diagnostics;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly MatchesService matchesService;

        public HomeController(MatchesService matchesService)
        {
            this.matchesService = matchesService;
        }

        public async Task<IActionResult> Index()
        {
            var ongoingMatches = await matchesService.OngoingMatches();

            Matches matches = new Matches();
            foreach (var match in ongoingMatches)
            {
                matches.MatchScores.Add(MatchScore.ConvertMatchToMatchScore(match));
            }

            return View(matches);
        }
    }
}
