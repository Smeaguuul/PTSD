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

        [AfterScenario]
        public async Task AfterScenarioAsync()
        {
            //TODO: implement logic that has to run after executing each scenario
            // Deletes the game we used to test, from the database.
            await _matchService.DeleteMatch(_currentMatch.Id);
        }
    }
}