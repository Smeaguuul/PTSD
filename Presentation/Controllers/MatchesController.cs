using Business.Interfaces;
using DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchesService matchesService;

        public MatchesController(IMatchesService matchesService)
        {
            this.matchesService = matchesService;
        }

        [HttpGet]
        public async Task<Matches> GetMatches()
        {
            //await matchesService.SeedMatchData();

            var ongoingMatches = await matchesService.OngoingMatches();

            Matches matches = new Matches();
            foreach (var match in ongoingMatches)
            {
                var matchScore = await matchesService.GetMatchScore(match.Id);
                matches.MatchScores.Add(matchScore);
            }
            return matches;
        }
    }
}
