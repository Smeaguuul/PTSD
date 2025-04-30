using DTO;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class AdminController : Controller
    {
        public AdminController()
        {

        }
        public ActionResult Admin ()
        {
            var fields = new Field[3];
            fields[0] = new DTO.Field(1);
            fields[1] = new DTO.Field(2);
            fields[2] = new DTO.Field(3);
            Team team1 = new Team();
            team1.Players = new List<Player>() { new Player("Ole", 1), new Player("Kim", 2) };
            fields[2].CurrentMatch = new DTO.Match(team1, team1, DateOnly.FromDateTime(DateTime.Today), Status.Ongoing, 1);
            ViewBag.Fields = fields;
            return View();
        }
        public ActionResult AdminBtn(int fieldId, int matchId)
        {
            var fields = new Field[3];
            fields[0] = new DTO.Field(1);
            fields[1] = new DTO.Field(2);
            fields[2] = new DTO.Field(3);
            Team team1 = new Team();
            team1.Players = new List<Player>() { new Player("Ole", 1), new Player("Kim", 2) };
            Club club = new Club();
            club.Name = "Pakhus77";
            team1.Club = club;
            Match[] matchesTest = { new Match(team1, team1, DateOnly.FromDateTime(DateTime.Today), Status.Scheduled, 2), new Match(team1, team1, DateOnly.FromDateTime(DateTime.Today), Status.Scheduled, 1) };
            foreach (var match in matchesTest)
            {
                if (match.Id == matchId) {
                    match.Status = Status.Ongoing;
                    Field field = (Field)fields.Where(f => f.Id == fieldId);
                    match.Field = field;
                }
            }
            return RedirectToAction("Admin");
        }
        public ActionResult StartGame()
        {
            Team team1 = new Team();
            team1.Players = new List<Player>() { new Player("Ole", 1), new Player("Kim", 2) };
            Club club = new Club();
            club.Name = "Pakhus77";
            team1.Club = club;
            Match[] matchesTest = { new Match(team1, team1, DateOnly.FromDateTime(DateTime.Today), Status.Scheduled, 2), new Match(team1, team1, DateOnly.FromDateTime(DateTime.Today), Status.Scheduled, 1) };
            ViewBag.Matches = matchesTest;
            //ViewBag.FieldId = fieldId;
            //Console.WriteLine(fieldId);
            return View();
        }

      
    }
}
