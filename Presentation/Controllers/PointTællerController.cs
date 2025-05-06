using Business.Interfaces;
using DTO;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class PointTællerController : Controller
    {
        private readonly IMatchesService matchesService;

        public PointTællerController(IMatchesService matchesService)
        {
            this.matchesService = matchesService;
        }

        public async Task<IActionResult> Index()
        {
            var ongoingMatches = await matchesService.OngoingMatches();

            List<MatchScore> matchScores = new List<MatchScore>();
            foreach (var match in ongoingMatches)
            {
                var score = await matchesService.GetMatchScore(match.Id);
                matchScores.Add(score);
            }

            return View(matchScores); // View forventer IEnumerable<MatchScore>
        }
    }
}
