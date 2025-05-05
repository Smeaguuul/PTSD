using System.Threading.Tasks;
using Business.Services;
using DTO;
using Reqnroll;

namespace Tests.StepDefinitions
{
    [Binding]
    public sealed class MatchPointSteps
    {
        
        private readonly MatchesService _matchService;
        private Match _currentMatch;
        private readonly ClubsService _clubsService;

        public MatchPointSteps(MatchesService matchService, ClubsService clubsService)
        {
            _matchService = matchService;
            _clubsService = clubsService;
        }

        [BeforeScenario("@tag1")]
        public async Task BeforeScenarioWithTag()
        {
            var clubs = await _clubsService.GetAll();
            var homeTeamId = clubs.Where(club => club.Name.Equals("Pakhus77")).First().Teams[0].Id;
            var awayTeamId = clubs.Where(club => club.Name != "Pakhus77").First().Teams[0].Id;
            await _matchService.CreateMatch(homeTeamId, awayTeamId, DateOnly.FromDateTime(DateTime.Now), Status.Scheduled);
            var scheduledMatches = await _matchService.ScheduledMatches();
            _currentMatch = scheduledMatches.FirstOrDefault(match => match.Date.Equals(DateOnly.FromDateTime(DateTime.Now)) && match.HomeTeam.Id == homeTeamId && match.AwayTeam.Id == awayTeamId);
            await _matchService.StartMatch(_currentMatch.Id, true, 3);
        }

        [Given(@"Team A has (.*) points won")]
        public async Task GivenTeamAHasPointsWonAsync(int points)
        {
            for (int i = 0; i < points; i++)
            {
                await _matchService.UpdateMatchScore(_currentMatch.Id, true);
            }
        }

        [Given(@"Team B has (.*) points won")]
        public async Task GivenTeamBHasPointsWonAsync(int points)
        {
            for (int i = 0; i < points; i++)
            {
                await _matchService.UpdateMatchScore(_currentMatch.Id, false);
            }
        }

        [When(@"Team (A|B) wins the next (.*) points")]
        public async Task WhenTeamAWinsTheNextTwoPoints(string teamLetter, int points)
        {
            var winner = teamLetter == "A";
            for (int i = 0; i < points; i++)
            {
                await _matchService.UpdateMatchScore(_currentMatch.Id, winner);
            }
        }

        [Then(@"The point count for Team (A|B) should be (.*)")]
        public async Task ThenTheSetCountForTeamAShouldRemainAsync(string teamLetter, string expectedPoints)
        {
            var matchScore =  await _matchService.GetMatchScore(_currentMatch.Id);
            var actualPoints = (teamLetter == "A") ? matchScore.PointsHome : matchScore.PointsAway;
            Assert.AreEqual(expectedPoints.ToString(), actualPoints);
        }

        [Then("the match should move to a new game")]
        public async Task ThenTheMatchShouldMoveToANewGame()
        {
            var matchScore = await _matchService.GetMatchScore(_currentMatch.Id);
            Assert.AreEqual("0", matchScore.PointsAway);
            Assert.AreEqual("0", matchScore.PointsHome);
        }

        [Then("the set count for Team A should be {int}")]
        public async Task ThenTheSetCountForTeamAShouldBeAsync(int p0)
        {
            var matchScore = await _matchService.GetMatchScore(_currentMatch.Id);
            Assert.AreEqual(p0, matchScore.GamesThisSetHome);
        }

        [Then("the set count for Team B should be {int}")]
        public async Task ThenTheSetCountForTeamBShouldBeAsync(int p0)
        {
            var matchScore = await _matchService.GetMatchScore(_currentMatch.Id);
            Assert.AreEqual(p0, matchScore.GamesThisSetAway);
        }

        [Given("Team A has {int} set won")]
        public async Task GivenTeamAHasSetWonAsync(int p0)
        {
            var matchScore = await _matchService.GetMatchScore(_currentMatch.Id);
            for (int i = 0; i < p0; i++)
            {
                while (matchScore.SetsHome < p0)
                {
                    await _matchService.UpdateMatchScore(_currentMatch.Id, true);
                    matchScore = await _matchService.GetMatchScore(_currentMatch.Id);
                }
            }
        }

        [Given("Team B has {int} set won")]
        public async Task GivenTeamBHasSetWonAsync(int p0)
        {
            var matchScore = await _matchService.GetMatchScore(_currentMatch.Id);
            for (int i = 0; i < p0; i++)
            {
                while (matchScore.SetsAway < p0)
                {
                    await _matchService.UpdateMatchScore(_currentMatch.Id, false);
                    matchScore = await _matchService.GetMatchScore(_currentMatch.Id);
                }
            }
        }

        [Given("the current set score is {int}-{int} in favor of Team A")]
        public async Task GivenTheCurrentSetScoreIsInFavorOfTeamAAsync(int p0, int p1)
        {
            var matchScore = await _matchService.GetMatchScore(_currentMatch.Id);
            for (int i = 0; i < p0; i++)
            {
                while (matchScore.GamesThisSetHome < p0)
                {
                    await _matchService.UpdateMatchScore(_currentMatch.Id, true);
                    matchScore = await _matchService.GetMatchScore(_currentMatch.Id);
                }
            }
            for (int i = 0; i < p0; i++)
            {
                while (matchScore.GamesThisSetAway < p1)
                {
                    await _matchService.UpdateMatchScore(_currentMatch.Id, false);
                    matchScore = await _matchService.GetMatchScore(_currentMatch.Id);
                }
            }
        }

        [Then("the match should move to the next set")]
        public async Task ThenTheMatchShouldMoveToTheNextSetAsync()
        {
            var matchScore = await _matchService.GetMatchScore(_currentMatch.Id);
            Assert.AreEqual(0, matchScore.GamesThisSetAway + matchScore.GamesThisSetHome);
            Assert.AreEqual("0", matchScore.PointsAway);
            Assert.AreEqual("0", matchScore.PointsHome);
        }

        [Then("the current set score should be reset to {int}-{int}")]
        public async Task ThenTheCurrentSetScoreShouldBeResetToAsync(int p0, int p1)
        {
            var matchScore = await _matchService.GetMatchScore(_currentMatch.Id);
            Assert.AreEqual(p0, matchScore.GamesThisSetHome);
            Assert.AreEqual(p1, matchScore.GamesThisSetAway);
        }

        [Then("the won set count for Team A should be {int}")]
        public async Task ThenTheWonSetCountForTeamAShouldBeAsync(int p0)
        {
            var matchScore = await _matchService.GetMatchScore(_currentMatch.Id);
            Assert.AreEqual(p0, matchScore.SetsHome);
        }

        [Then("the won set count for Team B should be {int}")]
        public async Task ThenTheWonSetCountForTeamBShouldBeAsync(int p0)
        {
            var matchScore = await _matchService.GetMatchScore(_currentMatch.Id);
            Assert.AreEqual(p0, matchScore.SetsAway);
        }




        [AfterScenario]
        public async Task AfterScenarioAsync()
        {
            //TODO: implement logic that has to run after executing each scenario
            // Deletes the game we used to test, from the database.
            await _matchService.DeleteMatch(_currentMatch.Id);
        }
    }
}