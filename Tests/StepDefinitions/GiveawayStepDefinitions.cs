using System;
using System.Threading.Tasks;
using Business.Services;
using DataAccess.Models.Giveaways;
using Reqnroll;

namespace Tests.StepDefinitions
{
    [Binding]
    public class GiveawayStepDefinitions
    {
        string name;
        string mail;
        string desc;
        DateTime startDate;
        DateTime endDate;
        int id;
        private Exception caughtException;
        private readonly GiveawayService giveawayService;

        public GiveawayStepDefinitions(GiveawayService giveawayService)
        {
            this.giveawayService = giveawayService;
        }


        [Given("name is (.*)")]
        public void GivenNameIs(string name)
        {
            this.name = name;
        }

        [Given("a description is (.*)")]
        public void GivenADescriptionIs(string desc)
        {
            this.desc = desc;
        }

        [Given("a start date is (.*)")]
        public void GivenAStartDateIs(string date)
        {
            if (date.Equals("past", StringComparison.OrdinalIgnoreCase))
            {
                startDate = DateTime.Now.AddDays(-1);
            }
            else
            {
                startDate = DateTime.Parse(date);
            }
        }

        [Given("an end date is {DateTime}")]
        public void GivenAnEndDateIs(DateTime endDate)
        {
            this.endDate = endDate;
        }

        [When("you create a giveaway")]
        public async Task WhenYouCreateAGiveawayAsync()
        {
            await giveawayService.CreateGiveawayAsync(name, desc, startDate, endDate);
        }

        [Then("the giveaway should be created")]
        public async Task ThenTheGiveawayShouldBeCreated()
        {
            var giveaways = await giveawayService.GetGiveaways();

            Assert.IsNotNull(giveaways);
            id = giveaways.FirstOrDefault(giveaway => giveaway.Name == name).Id;
            Assert.IsTrue(giveaways.Count(giveaway => giveaway.Name == name) == 1);
        }

        [When("you create a giveaway with early date")]
        public async Task WhenYouCreateAGiveawayWithEarlyDate()
        {
            caughtException = await Assert.ThrowsAsync<ArgumentException>(() =>
                giveawayService.CreateGiveawayAsync(name, desc, startDate, endDate));
        }



        [Then("An error should be thrown with message \"Start date cannot be in the past\"")]
        public async Task ThenTheGiveawayShouldNotBeCreatedAsync()
        {
            var giveaways = await giveawayService.GetGiveaways();
            Assert.IsFalse(giveaways.Any(g => g.Name == name));
            Assert.AreEqual("Start date cannot be in the past", caughtException.Message);
        }


        [When("you create a giveaway with end date before start date")]
        public async Task WhenYouCreateAGiveawayWithEndDateBeforeStartDate()
        {
            caughtException = await Assert.ThrowsAsync<ArgumentException>(() =>
                giveawayService.CreateGiveawayAsync(name, desc, startDate, endDate));
        }

        [Then("an error should be thrown with message {string}")]
        public void ThenAnErrorShouldBeThrownWithMessage(string p0)
        {
           var giveaways = giveawayService.GetGiveaways().Result;
            Assert.IsFalse(giveaways.Any(g => g.Name == name));
            Assert.AreEqual("End date cannot be before start date", caughtException.Message);
        }

        [Given("a giveaway with name DupGA and active dates exists")]
        public async Task GivenAGiveawayWithNameDupGAAndActiveDatesExists()
        {
           await giveawayService.CreateGiveawayAsync("DupGA", "DupGADESC", DateTime.Now.AddDays(1), DateTime.Now.AddDays(2));
        }

        [Given("a contestant with email {string} and name {string} is added to a giveaway")]
        public async Task GivenAContestantWithEmailIsAddedToTheGiveaway(string mail, string name)
        {
            this.name = name;
            this.mail = mail;

            id = giveawayService.GetGiveaways().Result.FirstOrDefault(g => g.Name == "DupGA").Id;
          await giveawayService.AddContestantToGiveawayAsync(id, mail, name);

        }

        bool resultOfAddingTwo;
        [When("you add the same contestant again")]
        public async Task WhenYouAddTheSameContestantAgain()
        {
            resultOfAddingTwo = await giveawayService.AddContestantToGiveawayAsync(id, mail, name);
        }

        [Then("it should return false and no contestant added")]
        public void ThenTheResultShouldBeFalse()
        {
            var countOfDuplicateContestant = giveawayService.GetContestants(id).Result.Count(a => a.Email == mail);
            Assert.AreEqual(1, countOfDuplicateContestant);
            Assert.AreEqual(resultOfAddingTwo,false);
        }

        [Given("a giveaway with name WinGA and {int} contestants exists")]
        public async Task GivenAGiveawayWithNameWinGAAndContestantsExists(int amount)
        {
            await giveawayService.CreateGiveawayAsync("WinGA", "WinGADESC", DateTime.Now.AddDays(1), DateTime.Now.AddDays(2));

            id = giveawayService.GetGiveaways().Result.FirstOrDefault(g => g.Name == "WinGA").Id;

            for (int i = amount - 1; i >= 0; i--)
            {
                await giveawayService.AddContestantToGiveawayAsync(id,i+"mail@gmail.com",i+"name");
            }
        }
        List<DTO.Giveaway.ContestantDto> winners = new List<DTO.Giveaway.ContestantDto>();
        [When("you pick one winner")]
        public async Task WhenYouPickOneWinner()
        {
          winners.Add(await giveawayService.PickWinner(id));
        }

        [Then("one winner should be returned")]
        public void ThenOneWinnerShouldBeReturned()
        {
            Assert.AreEqual(winners.Count,1);
        }

        [Given("a giveaway with name RemoveGA and one contestant exists")]
        public async Task GivenAGiveawayWithNameRemoveGAAndOneContestantExists()
        {
            name = "Thor";
            mail = "test@mail.com";
            await giveawayService.CreateGiveawayAsync("RemoveGA", "RemoveGADESC", DateTime.Now.AddDays(1), DateTime.Now.AddDays(2));
            id = giveawayService.GetGiveaways().Result.FirstOrDefault(g => g.Name == "RemoveGA").Id;
            await giveawayService.AddContestantToGiveawayAsync(id, mail, name);
        }

        [When("you remove the contestant")]
        public async Task WhenYouRemoveTheContestant()
        {
            var contestantId = giveawayService.GetContestants(id).Result.FirstOrDefault(c => c.Name == name).Id;
            await giveawayService.RemoveContestantFromGiveawayAsync(id,contestantId);
        }

        [Then("the contestant should be removed from the giveaway")]
        public void ThenTheContestantShouldBeRemovedFromTheGiveaway()
        {

            Assert.IsNull(giveawayService.GetContestants(id).Result.FirstOrDefault(c => c.Name == name));
        }





        [AfterScenario("@GiveawayTag")]
        public async Task AfterScenarioAsync()
        {
            await giveawayService.DeleteGiveaway(id);

        }
    }
}
