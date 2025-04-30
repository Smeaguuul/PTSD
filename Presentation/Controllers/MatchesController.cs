using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DTO;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetMatches()
        {
            string matches = "Matches";
            return Ok(matches);
        }
    }
}
