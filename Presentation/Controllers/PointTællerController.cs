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

        // Overblik over alle planlagte kampe
        public async Task<IActionResult> Overblik()
        {
            var kampe = await _matchesService.ScheduledMatches();
            return View(kampe.ToList());
        }

        // Vis pointtæller for én kamp
        public async Task<IActionResult> Index(int id)
        {
            var kampe = await _matchesService.ScheduledMatches();
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
                    // Hvis lige, tæller vi ikke nogen (kan tilpasses)
                }


                setScores.Add((teamOneScore, teamTwoScore));
            }

            var viewModel = new MatchViewModel
            {
                Match = kamp,
                SetScores = setScores
            };

            return View(viewModel);
        }
    }
}
