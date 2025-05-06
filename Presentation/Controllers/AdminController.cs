using Microsoft.AspNetCore.Mvc;
using DTO;
using Business.Interfaces;
using Microsoft.EntityFrameworkCore;
using Presentation.Models;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using DTO.Giveaway;
using System.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;



namespace Presentation.Controllers
{
    public class AdminController : Controller
    {
        private readonly IMatchesService matchesService;
        private readonly IClubsService clubsService;
        private readonly IGiveawayService giveawayService;
        private readonly IAdminUserService adminUserService;

        public AdminController(IMatchesService matchesService, IClubsService clubsService, IGiveawayService giveawayService)
        {
            this.matchesService = matchesService;
            this.clubsService = clubsService;
            this.giveawayService = giveawayService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await adminUserService.getAdminUser(username);
            
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)){
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Index", "admin");
            }

            ViewBag.Error = "Invalid credentials";
            return View();
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Login");
        }

        public ActionResult Generate(string url)
        {
            //if (password != "Johans sista")
            //{
            //    return Forbid();
            //}

            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeAsPng = qrCode.GetGraphic(20);

            return File(qrCodeAsPng, "image/png");
        }
        public ActionResult Qr(int id)
        {

            string url = $"http://localhost:5023/pointmanager?Id={id}";
            ViewBag.QrImageUrl = Url.Action("Generate", "Admin", new { url = url});
            ViewBag.OriginalUrl = url;
            return View();
        }
        [HttpPost]
        public ActionResult CreateGiveaway(CreateGiveawayDto newGiveaway)
        {
            if (!ModelState.IsValid)
            {
                // You can return the same page with current model to show errors
                var model = new AdminGiveawayPageViewModel
                {
                    Giveaways = giveawayService.GetGiveaways().Result.ToList(),
                    NewGiveaway = newGiveaway
                };
                return View("AdminGiveaway", model);
            }

            giveawayService.CreateGiveawayAsync(newGiveaway);
            return RedirectToAction("AdminGiveaway");
        }

        [HttpPost]
        public async Task<IActionResult> AddContestant(int giveawayId, string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
            {
                TempData["Error"] = "Name and Email are required.";
                return RedirectToAction("AdminGiveaway");
            }

            await giveawayService.AddContestantToGiveawayAsync(giveawayId, name, email);
            return RedirectToAction("AdminGiveaway");
        }


        public async Task<IActionResult> ViewContestants(int giveawayId)
        {
            var giveaway = (await giveawayService.GetGiveaways()).FirstOrDefault(g => g.Id == giveawayId);
            if (giveaway == null)
            {
                return NotFound();
            }

            return View(giveaway); // Pass GiveawayDto which includes Contestants
        }


        [HttpPost]
        public async Task<IActionResult> EndGiveaway(int giveawayId)
        {
            await giveawayService.DeleteGiveaway(giveawayId);
            return RedirectToAction("AdminGiveaway");
        }

        [HttpPost]
        public async Task<IActionResult> PickWinners(int giveawayId, int numberOfWinners)
        {
            await giveawayService.PickWinner(giveawayId, numberOfWinners);
            return RedirectToAction("AdminGiveaway");
        }


        public async Task<ActionResult> AdminGiveaway()
        {
            var giveaways = await giveawayService.GetGiveaways();

            var model = new AdminGiveawayPageViewModel
            {
                Giveaways = giveaways.ToList() // Ensure this is not null
            };

            return View(model); // strongly typed
        }

        [Authorize]
        public async Task<ActionResult> Index()
        {
            var matches = await matchesService.ScheduledMatches();
            AdminHomepage model = new AdminHomepage() { Matches = [.. matches] };
            return View(model);
        }

        [Authorize]
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
            string url = $"http://localhost:5023/Pointmanager/PickServer?matchId={matchId}&fieldId={fieldId}";
            ViewBag.QrImageUrl = Url.Action("Generate", "Admin", new { url = url});
            ViewBag.OriginalUrl = url;
            return View("StartGame");
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

        public async Task<IActionResult> EditGame(int matchId, int setsHome, int setsAway)
        {
            try
            {
                await matchesService.ChangeFinishedGameScore(matchId, setsHome, setsAway);
                ViewBag.Result = "Game score updated successfully!";
                var match = await matchesService.GetMatch(matchId);
                var matchScore = await matchesService.GetMatchScore(match.Id);
                var model = new MatchInfo() { Match = match, MatchScore = matchScore };
                return View("GameEditorIndividual", model);
            }
            catch (Exception e)
            {
                ViewBag.Message = "Something went wrong: " + e.Message;
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
        public async Task<ActionResult> EndMatchBtn(int matchId)
        {

            await matchesService.EndMatch(matchId);
            return RedirectToAction("Admin");
        }


        public async Task<IActionResult> Clubs()
        {
            var TeamMessage = TempData["TeamMessage"] as string;
            var ClubMessage = TempData["ClubMessage"] as string;
            ViewBag.ClubMessage = ClubMessage;
            ViewBag.TeamMessage = TeamMessage;
            var clubs = await clubsService.GetAll();
            return View(clubs);
        }

        [HttpPost]
        public ActionResult CreateTeam(string TeamName, string ClubAbbreviation, string Player1Name, string Player2Name)
        {
            TempData["TeamMessage"] = "Creation Successful";
            return RedirectToAction("Clubs");
        }

        [HttpPost]
        public ActionResult CreateClub(string ClubName, string Location, string Abbreviation)
        {
            TempData["ClubMessage"] = "Creation Successful";
            return RedirectToAction("Clubs");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string oldPassword, string newPassword)
        {
            try
            {
                adminUserService.changePassword("admin", oldPassword, newPassword);
                ViewBag.Message = "Password changed successfully!";
            }
            catch (Exception e)
            {
                ViewBag.Error = "Error: " + e.Message;
            }
            return View();
        }
    }
}
