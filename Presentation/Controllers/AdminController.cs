using Microsoft.AspNetCore.Mvc;
using DTO;
using Business.Services;
using Microsoft.EntityFrameworkCore;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class AdminController : Controller
    {
        //testdata
        Field[] fields = { new DTO.Field(1), new DTO.Field(2), new DTO.Field(3) };

        private readonly MatchesService matchesService;
        private readonly ClubsService clubsService;
        public AdminController(MatchesService matchesService, ClubsService clubsService)
        {
            this.matchesService = matchesService;
            this.clubsService = clubsService;
        }
        public async Task<ActionResult> Admin()
        {
            Field[] fields = { new DTO.Field(1), new DTO.Field(2), new DTO.Field(3) };
            var ongoingMatches = await matchesService.OngoingMatches();
            foreach (var field in fields)
            {
                foreach (var match in ongoingMatches)
                {
                    if (field.Id == match.Field.Id)
                        field.CurrentMatch = match;
                }
            }
            ViewBag.Fields = fields;
            return View();
        }

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

        public async Task<ActionResult> StartGame(int fieldId)
        {
            var scheduledMatches = await matchesService.ScheduledMatches();


            //Team team1 = new Team();
            //team1.Players = new List<Player>() { new Player("Ole", 1), new Player("Kim", 2) };
            //Club club = new Club();
            //club.Name = "Pakhus77";
            //team1.Club = club;
            //Match[] matchesTest = { new Match(team1, team1, DateOnly.FromDateTime(DateTime.Today), Status.Scheduled, 2), new Match(team1, team1, DateOnly.FromDateTime(DateTime.Today), Status.Scheduled, 1) };
            ViewBag.Matches = scheduledMatches;
            ViewBag.FieldId = fieldId;
            return View();
        }
        public ActionResult StartGameForm()
        {

            return RedirectToAction("Admin");
        }

        public async Task<ActionResult> AddMatch()
        {
            var clubs = await clubsService.GetAll();
            foreach (var club in clubs)
            {
                club.Teams.ForEach(t => t.Club = null);
            }
            List<Field> fields = [new DTO.Field(1), new DTO.Field(2), new DTO.Field(3)];
            var viewModel = new AddMatchModel()
            {
                Clubs = clubs.ToList(), // Fetch clubs and their teams
                Fields = fields
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddMatch(AddMatchModel model)
        {
            if (model.SelectedHomeTeamId <= 0 || model.SelectedAwayTeamId <= 0 || model.Date == default || string.IsNullOrEmpty(model.Status.ToString()))
            {
                // If model state is invalid, repopulate the ViewModel
                var clubs = await clubsService.GetAll();
                List<Field> fields = [new DTO.Field(1), new DTO.Field(2), new DTO.Field(3)];
                var viewModel = new AddMatchModel()
                {
                    Clubs = clubs.ToList(), // Fetch clubs and their teams
                    Fields = fields
                };

                return View(viewModel);
            }
            await matchesService.CreateMatch(model.SelectedHomeTeamId, model.SelectedAwayTeamId, model.Date, model.Status);

            return RedirectToAction("StartGame"); // Redirect to a suitable action

        }

        public ActionResult UploadAd()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UploadAd(IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    string fileExtension = Path.GetExtension(file.FileName);
                    string[] allowedExtensions = { ".jpg", ".png" };

                    if (allowedExtensions.Contains(fileExtension))
                    {
                        var maxFileSize = 20 * 1024 * 1024;
                        if (file.Length > maxFileSize)
                        {
                            ViewBag.Message = "File size exceeds the maximum limit of 20MB.";
                            return View();
                        }

                        // Define the path to save the uploaded file
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                        // Ensure the uploads directory exists
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        // Save the file to the server
                        //var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(uploadsFolder, "Ad" + fileExtension);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Display success message
                        ViewBag.Message = "File uploaded successfully!";
                    }
                    else
                    {
                        ViewBag.Message = "Only jpg, and png files are allowed.";
                    }
                }
                else
                {
                    // Display error message
                    ViewBag.Message = "Please select a file to upload.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }

            return View();
        }
    }
}
