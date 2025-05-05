using Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class ClubsController : Controller
    {
        private readonly ClubsService _clubsService;

        public ClubsController(ClubsService clubsService)
        {
            _clubsService = clubsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string name, string abbreviation, string location)
        {
            //await _clubsService.CreateClub(name, abbreviation, location);
            return View();
        }
    }
}
