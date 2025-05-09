﻿using Microsoft.AspNetCore.Mvc;
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
using Business.Services;
using System.Threading.Tasks;
using System.Net.Sockets;



namespace Presentation.Controllers
{
    public class AdminController : Controller
    {
        private readonly IMatchesService matchesService;
        private readonly IClubsService clubsService;
        private readonly IGiveawayService giveawayService;
        private readonly IAdminUserService adminUserService;

        public AdminController(IMatchesService matchesService, IClubsService clubsService, IGiveawayService giveawayService, IAdminUserService adminUserService)
        {
            this.matchesService = matchesService;
            this.clubsService = clubsService;
            this.giveawayService = giveawayService;
            this.adminUserService = adminUserService;
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
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeAsPng = qrCode.GetGraphic(20);

            return File(qrCodeAsPng, "image/png");
        }
        [Authorize]
        public async Task<ActionResult> Qr(int id)
        {
            var ongoingMatches = await matchesService.OngoingMatches();
            ViewBag.Matches = ongoingMatches;

            string localIP = "localhost";
            string hostName = Dns.GetHostName();
            IPAddress[] ipAddresses = Dns.GetHostAddresses(hostName);
            foreach (var ip in ipAddresses)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            string url = $"http://{localIP}:5023/pointmanager?Id={id}";
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

        [Authorize]
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
            string localIP = "localhost";
            string hostName = Dns.GetHostName();
            IPAddress[] ipAddresses = Dns.GetHostAddresses(hostName);
            foreach (var ip in ipAddresses)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            string url = $"http://{localIP}:5023/Pointmanager/PickServer?matchId={matchId}&fieldId={fieldId}";
            
            ViewBag.QrImageUrl = Url.Action("Generate", "Admin", new { url = url});
            ViewBag.OriginalUrl = url;
            return View("StartGame");
        }

        [Authorize]
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

        [Authorize]
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
                var clubs = await clubsService.GetAll();
                List<Field> fields = [new DTO.Field(1), new DTO.Field(2), new DTO.Field(3)];
                var viewModel = new AddMatchModel()
                {
                    Clubs = clubs.ToList(),
                    Fields = fields
                };

                return RedirectToAction("AddMatch");
            }
            await matchesService.CreateMatch(model.SelectedHomeTeamId, model.SelectedAwayTeamId, model.Date, model.Status);

            return RedirectToAction("AddMatch");

        }
        [Authorize]
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
        

        [Authorize]
        public IActionResult UploadAd()
        {
            var adDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Ads");
            if (!Directory.Exists(adDir))
                Directory.CreateDirectory(adDir);

            var adImages = Directory.GetFiles(adDir)
                .Select(Path.GetFileName)
                .Where(f => f.EndsWith(".jpg") || f.EndsWith(".png"))
                .ToList();

            ViewBag.AdImages = adImages;
            return View();
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> UploadAd(IFormFile file)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Ads");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            try
            {
                if (file != null && file.Length > 0)
                {
                    string fileExtension = Path.GetExtension(file.FileName).ToLower();
                    string[] allowedExtensions = { ".jpg", ".png" };

                    if (allowedExtensions.Contains(fileExtension))
                    {
                        var uniqueFileName = Path.GetFileName(file.FileName); // evt. tilføj timestamp hvis du vil undgå dubletter
                        var path = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        ViewBag.Message = "Filen blev uploadet!";
                    }
                    else
                    {
                        ViewBag.Message = "Kun JPG og PNG filer er tilladt.";
                    }
                }
                else
                {
                    ViewBag.Message = "Vælg venligst en fil.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Fejl: " + ex.Message;
            }

            var adImages = Directory.GetFiles(uploadsFolder)
                .Select(Path.GetFileName)
                .Where(f => f.EndsWith(".jpg") || f.EndsWith(".png"))
                .ToList();

            ViewBag.AdImages = adImages;
            return View();
        }

        [Authorize, HttpPost]
        public IActionResult DeleteAd(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Ads", fileName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                ViewBag.Message = "Filen blev slettet.";
            }
            else
            {
                ViewBag.Message = "Filen blev ikke fundet.";
            }

            var adImages = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Ads"))
                .Select(Path.GetFileName)
                .Where(f => f.EndsWith(".jpg") || f.EndsWith(".png"))
                .ToList();

            ViewBag.AdImages = adImages;
            return View("UploadAd");
        }






        public async Task<ActionResult> EndMatchBtn(int matchId)
        {

            await matchesService.EndMatch(matchId);
            return RedirectToAction("Admin");
        }

        [Authorize]
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
        public async Task<ActionResult> AddTeamToClub(string TeamName, string ClubAbbreviation, string Player1Name, string Player2Name)
        {
            await clubsService.AddTeamToClub(TeamName, Player1Name, Player2Name, ClubAbbreviation);
            TempData["TeamMessage"] = "Creation Successful";
            return RedirectToAction("Clubs");
        }

        [HttpPost]
        public async Task<ActionResult> CreateClub(string ClubName, string Location, string Abbreviation)
        {
            await clubsService.CreateClub(ClubName, Location, Abbreviation);
            TempData["ClubMessage"] = "Creation Successful";
            return RedirectToAction("Clubs");
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ChangePasswordAsync(string oldPassword, string newPassword)
        {
            try
            {
                await adminUserService.changePassword("admin", oldPassword, newPassword);
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
