using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class PointManagerController : Controller
    {

        private readonly IMatchesService _matchesService;

        public PointManagerController(IMatchesService matchesService)
        {
            _matchesService = matchesService;
        }
        public async Task<IActionResult> Index(int Id)
        {
            var matchScore = await _matchesService.GetMatchScore(Id);
            return View(matchScore);
        }

        public async Task<IActionResult> StartMatch(int matchId, bool server, int fieldId)
        {
            await _matchesService.StartMatch(matchId, server, fieldId);
            return Redirect($"/PointManager?Id={matchId}");
        }

        public async Task<IActionResult> PickServer(int matchId, int fieldId)
        {
            var match = await _matchesService.GetMatch(matchId);
            var model = new PickServerViewModel { Match = match, FieldId = fieldId };
            return View(model);
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
