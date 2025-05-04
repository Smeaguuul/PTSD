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

        // Overblik over hvilke baner der er ledige samt igangværende
        public async Task<ActionResult> Admin()
        {
            Field[] fields = { new Field(1), new Field(2), new Field(3) };
            var ongoingMatches = await matchesService.OngoingMatches();
            foreach (var field in fields)
            {
                foreach (var match in ongoingMatches)
                {
                    if(field.Id == match.Field.Id)
                        field.CurrentMatch = match;
                }
            }
            ViewBag.Fields = fields;
            return View();
        }

        // Ændrer status på kamp fra Scheduled til Ongoing
        public async Task<ActionResult> AdminBtn(int fieldId, int matchId)
        {
            var scheduledMatches = await matchesService.ScheduledMatches();
            // TODO Opdater i DB
            foreach (var match in scheduledMatches)
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

        // Overblik over planlagte kampe
        public async Task<ActionResult> StartGame(int fieldId)
        {
            var scheduledMatches = await matchesService.ScheduledMatches();

            ViewBag.Matches = scheduledMatches;
            ViewBag.FieldId = fieldId;
            return View();
        }
        public ActionResult StartGameForm() {

            return RedirectToAction("Admin");
        }
    }
}
