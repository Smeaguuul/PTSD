using Business.Interfaces;
using Business.Services;
using DTO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox; // or the browser you are using
using Reqnroll;

[Binding]
public class AdminSteps
{
    private Match _match;
    private IWebDriver _driver;
    private readonly MatchesService _matchService;
    private readonly ClubsService _clubsService;

    public AdminSteps(MatchesService matchService, ClubsService clubsService)
    {
        _matchService = matchService;
        _clubsService = clubsService;
    }

    [BeforeScenario("@MatchManagement")]
    public async Task BeforeScenarioWithTag()
    {
        var ongoingMatches = await _matchService.OngoingMatches();
        var matchesToDelete = ongoingMatches.Where(m => m.Field.Id == 3).ToList();
        foreach (var match in matchesToDelete)
        {
            await _matchService.DeleteMatch(match.Id);
        }
        var clubs = await _clubsService.GetAll();
        var homeTeamId = clubs.Where(club => club.Name.Equals("Pakhus77")).First().Teams[0].Id;
        var awayTeamId = clubs.Where(club => club.Name != "Pakhus77").First().Teams[0].Id;
        await _matchService.CreateMatch(homeTeamId, awayTeamId, DateOnly.FromDateTime(DateTime.Now), Status.Scheduled);
        var scheduledMatches = await _matchService.ScheduledMatches();
        _match = scheduledMatches.FirstOrDefault(match => match.Date.Equals(DateOnly.FromDateTime(DateTime.Now)) && match.HomeTeam.Id == homeTeamId && match.AwayTeam.Id == awayTeamId) ?? throw new Exception("Couldn't create a match.");
    }

    [Given(@"I navigate to the login page")]
    public void GivenINavigateToTheLoginPage()
    {
        _driver = new FirefoxDriver();
        _driver.Navigate().GoToUrl("http://localhost:5023/Admin/Login");
    }

    [When(@"I enter my credentials")]
    public void WhenIEnterMyCredentials(Reqnroll.Table table)
    {
        var data = table.Rows.ToDictionary(r => r["Field"], r => r["Value"]);
        _driver.FindElement(By.Id("username")).SendKeys(data["Username"]);
        _driver.FindElement(By.Id("password")).SendKeys(data["Password"]);
    }

    [When(@"I log in")]
    public void WhenILogIn()
    {
        _driver.FindElement(By.Id("login-button")).Click();
    }

    [Given("I am on the admin dashboard and logged in")]
    public void GivenIAmOnTheAdminDashboardAndLoggedIn()
    {
        _driver = new FirefoxDriver();
        _driver.Navigate().GoToUrl("http://localhost:5023/Admin/Login");
        _driver.FindElement(By.Id("username")).SendKeys("admin");
        _driver.FindElement(By.Id("password")).SendKeys("1234");
        _driver.FindElement(By.Id("login-button")).Click();
    }


    [Then(@"I should be on the dashboard")]
    public void ThenIShouldBeOnTheDashboard()
    {
        var expectedUrl = "http://localhost:5023/admin";
        var actualUrl = _driver.Url;
        Assert.AreEqual(expectedUrl, actualUrl);
    }

    [When(@"I navigate to the start match page")]
    public void WhenINavigateToTheStartMatchPage()
    {
        _driver.Navigate().GoToUrl("http://localhost:5023/Admin/Admin");
    }

    [When(@"I select a field to start a game")]
    public void WhenISelectAFieldToStartAGame()
    {
        _driver.FindElement(By.Id("startMatchBtn_3")).Click();
    }

    [When(@"I select a match to start")]
    public void WhenISelectAMatchToStart()
    {
        _driver.FindElement(By.Id($"link_{_match.Id}")).Click();

    }

    [Then(@"I should receive a QR code that refers to the point manager")]
    public void ThenIShouldReceiveAQRCodeThatRefersToThePointManager()
    {
        var qrCodeElement = _driver.FindElement(By.Id("qr-code"));
        Assert.IsNotNull(qrCodeElement);
        var qrCodeUrl = qrCodeElement.GetAttribute("src");
        Assert.Contains("Pointmanager", qrCodeUrl, "QR code doesn't point to point manager."); // Check that the URL contains "point-manager"
        Assert.Contains("PickServer", qrCodeUrl, "QR code doesn't point the pick-server page first.");
        Assert.Contains(_match.Id.ToString(), qrCodeUrl, "QR code doesn't point to the right match.");
    }

    [AfterScenario("@MatchManagement")]
    public async Task CleanUpAsync()
    {
        _driver.Quit();
        await _matchService.DeleteMatch(_match.Id);
    }


    [Given("Given the end user has a QR code for the point manager of a new match")]
    public void GivenGivenTheEndUserHasAQRCodeForThePointManagerOfANewMatch()
    {
        _driver = new FirefoxDriver();
    }

    [When("the end user scans the QR code and enters the link")]
    public void WhenTheEndUserScansTheQRCodeAndEntersTheLink()
    {
        _driver.Navigate().GoToUrl($"http://localhost:5023/Pointmanager/PickServer?matchId={_match.Id}&fieldId=3");
    }

    [Then("the first view should be the selection of who serves first")]
    public void ThenTheFirstViewShouldBeTheSelectionOfWhoServesFirst()
    {
        
        var homeServesButton = _driver.FindElement(By.Id("home-serve-btn"));
        Assert.IsNotNull(homeServesButton, "Doesn't get to 'pick server' page first.");        
        var awayServesButton = _driver.FindElement(By.Id("away-serve-btn"));
        Assert.IsNotNull(awayServesButton, "Doesn't get to 'pick server' page first.");
    }

    [Then("the next view should be the point manager")]
    public void ThenTheNextViewShouldBeThePointManager()
    {
        var homeServesButton = _driver.FindElement(By.Id("home-serve-btn"));
        homeServesButton.Click();

        var awayScoredButton = _driver.FindElement(By.Id("away-scores-btn"));
        Assert.IsNotNull(awayScoredButton);        
        var homeScoredButton = _driver.FindElement(By.Id("home-scores-btn"));
        Assert.IsNotNull(awayScoredButton);
    }

}
