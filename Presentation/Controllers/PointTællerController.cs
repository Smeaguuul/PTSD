using Microsoft.AspNetCore.Mvc;
using DTO;
using Business.Interfaces;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class PointTællerController : Controller
    {
        private readonly IMatchesService _matchesService;

        public PointTællerController(IMatchesService matchesService)
        {
            _matchesService = matchesService;
        }

        // 👉 NYT: Viser ALLE aktive kampe
        public async Task<IActionResult> Index()
        {
            var kampe = await _matchesService.OngoingMatches();

            var viewModels = new List<MatchViewModel>();

            foreach (var kamp in kampe)
            {
                var setScores = new List<(int TeamOneScore, int TeamTwoScore)>();

                foreach (var set in kamp.Score.Sets)
                {
                    int teamOneScore = 0;
                    int teamTwoScore = 0;

                    foreach (var game in set.Games)
                    {
                        int teamOnePoints = game.PointHistory.Count(p => p);
                        int teamTwoPoints = game.PointHistory.Count(p => !p);

                        if (teamOnePoints > teamTwoPoints)
                            teamOneScore++;
                        else if (teamTwoPoints > teamOnePoints)
                            teamTwoScore++;
                    }

                    setScores.Add((teamOneScore, teamTwoScore));
                }

                viewModels.Add(new MatchViewModel
                {
                    Match = kamp,
                    SetScores = setScores
                });
            }

            return View(viewModels); // Går til Index.cshtml med flere kampe
        }

        // 👉 Tilføj denne for enkelt kamp-visning (fx ved QR-scan)
        public async Task<IActionResult> Details(int id)
        {
            var kampe = await _matchesService.OngoingMatches();
            var kamp = kampe.FirstOrDefault(k => k.Id == id);

            if (kamp == null)
                return NotFound();

            var setScores = new List<(int TeamOneScore, int TeamTwoScore)>();

            foreach (var set in kamp.Score.Sets)
            {
                int teamOneScore = 0;
                int teamTwoScore = 0;

                foreach (var game in set.Games)
                {
                    int teamOnePoints = game.PointHistory.Count(p => p);
                    int teamTwoPoints = game.PointHistory.Count(p => !p);

                    if (teamOnePoints > teamTwoPoints)
                        teamOneScore++;
                    else if (teamTwoPoints > teamOnePoints)
                        teamTwoScore++;
                }

                setScores.Add((teamOneScore, teamTwoScore));
            }

            var viewModel = new MatchViewModel
            {
                Match = kamp,
                SetScores = setScores
            };

            return View("Details", viewModel);
        }
    }
}
