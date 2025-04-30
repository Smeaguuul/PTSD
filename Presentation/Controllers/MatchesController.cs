using Business.Services;
using DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly MatchesService matchesService;

        public MatchesController(MatchesService matchesService)
        {
            this.matchesService = matchesService;
        }

        [HttpGet]
        public async Task<IEnumerable<DTO.Match>> GetMatches()
        {
            return await matchesService.OngoingMatches();
        }
    }
}
