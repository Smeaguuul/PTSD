using Microsoft.AspNetCore.Mvc;
using DTO;
using Business.Services;

namespace Presentation.Controllers
{
    public class AdminController : Controller
    {
        //testdata
        Field[] fields = { new DTO.Field(1), new DTO.Field(2), new DTO.Field(3) };

        private readonly MatchesService matchesService;
        public AdminController(MatchesService matchesService)
        {
            this.matchesService = matchesService;
        }
        public ActionResult Admin()
        {
            //testdata
            Team team1 = new Team();
            team1.Players = new List<Player>() { new Player("Ole", 1), new Player("Kim", 2) };
            fields[2].CurrentMatch = new DTO.Match(team1, team1, DateOnly.FromDateTime(DateTime.Today), Status.Ongoing, 1);
            ViewBag.Fields = fields;
            return View();
        }

        public async Task<ActionResult> AdminBtn(int fieldId, int matchId)
        {            
            var ongoingMatches = await matchesService.OngoingMatches();
       
            foreach (var match in ongoingMatches)
            {
                if (match.Id == matchId)
                {
                    match.Status = Status.Ongoing;
                    foreach (var field in fields)
                    {
                        if (field.Id == fieldId)
                        {
                            match.Field = field;
                            field.CurrentMatch = match;
                        }
                    }
                }
            }

            return RedirectToAction("Admin");
        }

        public ActionResult StartGame(int fieldId)
        {
            //testdata
            Team team1 = new Team();
            team1.Players = new List<Player>() { new Player("Ole", 1), new Player("Kim", 2) };
            Club club = new Club();
            club.Name = "Pakhus77";
            team1.Club = club;
            Match[] matchesTest = { new Match(team1, team1, DateOnly.FromDateTime(DateTime.Today), Status.Scheduled, 2), new Match(team1, team1, DateOnly.FromDateTime(DateTime.Today), Status.Scheduled, 1) };
            ViewBag.Matches = matchesTest;
            ViewBag.FieldId = fieldId;
            return View();
        }
        public ActionResult StartGameForm() {

            return RedirectToAction("Admin");
        }
    }
}
