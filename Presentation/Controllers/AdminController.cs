using Microsoft.AspNetCore.Mvc;
using DTO;
using Business.Services;
using Microsoft.EntityFrameworkCore;
using Presentation.Models;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Presentation.Controllers
{
    public class AdminController : Controller
    {
        private readonly MatchesService matchesService;
        private readonly ClubsService clubsService;
        public AdminController(MatchesService matchesService, ClubsService clubsService)
        {
            this.matchesService = matchesService;
            this.clubsService = clubsService;
        }

        public ActionResult Generate(string url)
        {
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeAsPng = qrCode.GetGraphic(20);

            return File(qrCodeAsPng, "image/png");
        }
        public ActionResult Qr()
        {
            string url = "https://www.youtube.com/watch?v=dQw4w9WgXcQ";
            ViewBag.QrImageUrl = Url.Action("Generate", "Admin", new { url = url });
            ViewBag.OriginalUrl = url;
            return View();
        }

        public async Task<ActionResult> Index()
        {
            var matches = await matchesService.ScheduledMatches();
            AdminHomepage model = new AdminHomepage() { Matches = [.. matches] };
            return View(model);
        }
        public async Task<ActionResult> Admin()
        {
            Field[] fields = { new Field(1), new Field(2), new Field(3) };
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
            await matchesService.StartMatch(matchId, true, fieldId);

            return RedirectToAction("Admin");
        }

        public async Task<ActionResult> StartGame(int fieldId)
        {
            var scheduledMatches = await matchesService.ScheduledMatches();

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

        public async Task<ActionResult> GameEditor()
        {
            var matches = await matchesService.FinishedMatches();
            var model = new FinishedGames() { Matches = matches.ToList() };
            return View(model);
        }

        [Route("Admin/GameEditor/{id}")]
        public async Task<IActionResult> GameEditor(int Id)
        {
            Match match;
            try
            {
                match = await matchesService.GetMatch(Id);
                var matchScore = await matchesService.GetMatchScore(match.Id);
                var model = new MatchInfo() { Match = match, MatchScore = matchScore };
                return View("GameEditorIndividual", model);
            }
            catch
            {
                ViewBag.Message = "Something went wrong :(";
                return View("GameEditorIndividual");
            }
        }

        public async Task<IActionResult> EditGame(int Id)
        {
            Match match;
            try
            {
                match = await matchesService.GetMatch(Id);
                var matchScore = await matchesService.GetMatchScore(match.Id);
                var model = new MatchInfo() { Match = match, MatchScore = matchScore };
                return View("GameEditorIndividual", model);
            }
            catch
            {
                ViewBag.Message = "Something went wrong :(";
                return View("GameEditorIndividual");
            }
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
