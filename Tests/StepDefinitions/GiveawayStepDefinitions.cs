using System;
using System.Threading.Tasks;
using Business.Services;
using Reqnroll;

namespace Tests.StepDefinitions
{
    [Binding]
    public class GiveawayStepDefinitions
    {
        string name;
        string desc;
        DateTime startDate;
        DateTime endDate;
        int id;
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

        [Given("a start date is {DateTime}")]
        public void GivenAStartDateIs(DateTime startDate)
        {
            this.startDate = startDate;
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

        [AfterScenario("@GiveawayTag")]
        public async Task AfterScenarioAsync()
        {
            await giveawayService.DeleteGiveaway(id);

        }
    }
}
