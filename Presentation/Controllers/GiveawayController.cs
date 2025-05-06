using Business.Interfaces;
using DTO.Giveaway;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using Presentation.Models.Presentation.Models;

namespace Presentation.Controllers
{
    public class GiveawayController : Controller
    {
        private readonly IGiveawayService giveawayService;

        public GiveawayController(IGiveawayService giveawayService)
        {
            this.giveawayService = giveawayService;
        }

        [HttpGet]
        public async Task<IActionResult> Join()
        {
            var giveaways = await giveawayService.GetGiveaways();
            var model = new JoinGiveawayViewModel
            {
                Giveaways = giveaways.Select(g => new GiveawayDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    Description = g.Description,
                    StartDate = g.StartDate,
                    EndDate = g.EndDate
                }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Join(JoinGiveawayViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var giveaways = await giveawayService.GetGiveaways();
                model.Giveaways = giveaways.ToList();
                return View(model);
            }

            try
            {
                await giveawayService.AddContestantToGiveawayAsync(
                    model.JoinModel.GiveawayId,
                    model.JoinModel.Email,
                    model.JoinModel.Name);

                model.JoinModel.StatusMessage = "You have successfully joined the giveaway!";
            }
            catch (Exception ex)
            {
                model.JoinModel.StatusMessage = "Failed to join giveaway: " + ex.Message;
            }

            var updatedGiveaways = await giveawayService.GetGiveaways();
            model.Giveaways = updatedGiveaways.ToList();
            return View(model);
        }
    }
    }
