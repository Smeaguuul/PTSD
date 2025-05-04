using Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class PointManagerController : Controller
    {

        private readonly MatchesService _matchesService;

        public PointManagerController(MatchesService matchesService)
        {
            _matchesService = matchesService;
        }
        public async Task<IActionResult> Index(int Id)
        {
            var matchScore = await _matchesService.GetMatchScore(Id);
            return View(matchScore);
        }

        [HttpPost]
        public async Task<IActionResult> Index(bool pointScorer, int matchId)
        {
            await _matchesService.UpdateMatchScore(matchId, pointScorer);
            return Redirect($"/PointManager?Id={matchId}");
        }

        [HttpPost]
        public async Task<IActionResult> Undo(int matchId)
        {
            await _matchesService.UndoMatchPoint(matchId);
            return Redirect($"/PointManager?Id={matchId}");
        }
    }
}
