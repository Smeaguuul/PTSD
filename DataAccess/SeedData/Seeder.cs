using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Models.Giveaways;

namespace DataAccess.SeedData
{
    public class Seeder
    {
        private IRepository<AdminUser> _adminUserRepository;
        private IRepository<Match> _matchRepository;
        private IRepository<Club> _clubRepository;
        private IRepository<Giveaway> _giveawayRepository;

        public Seeder(IRepository<AdminUser> adminUserRepository, IRepository<Match> matchRepository, IRepository<Club> clubRepository, IRepository<Giveaway> giveawayRepository)
        {
            _adminUserRepository = adminUserRepository;
            _matchRepository = matchRepository;
            _clubRepository = clubRepository;
            _giveawayRepository = giveawayRepository;
        }

        public async Task SeedGenerator()
        {
            if (dataExists())
            {
                await SeedDataGiveAway();
                await SeedDataAdminUser();
                await SeedDataMatch();
            }
        }

        private async Task SeedDataAdminUser()
        {
            var adminUser = new AdminUser()
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("My69")
            };

            await _adminUserRepository.AddAsync(adminUser);

        }

        private async Task SeedDataGiveAway()
        {
            var giveaway1 = new Giveaway
            {
                Name = "Spring Giveaway",
                Description = "Win cool spring prizes!",
                StartDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now.AddDays(30)
            };

            var giveaway2 = new Giveaway
            {
                Name = "Summer Giveaway",
                Description = "Win cool summer prizes!",
                StartDate = DateTime.Now.AddDays(31),
                EndDate = DateTime.Now.AddDays(45)
            };

            var contestant1 = new Contestant { Name = "Alice", Email = "alice@example.com" };
            var contestant2 = new Contestant { Name = "Bob", Email = "bob@example.com" };
            var contestant3 = new Contestant { Name = "Charlie", Email = "charlie@example.com" };

            var giveawayContestant1 = new GiveawayContestant { contestant = contestant1, giveaway = giveaway1 };
            var giveawayContestant2 = new GiveawayContestant { contestant = contestant2, giveaway = giveaway1 };
            var giveawayContestant3 = new GiveawayContestant { contestant = contestant3, giveaway = giveaway2 };

            giveaway1.GiveawayContestants.Add(giveawayContestant1);
            giveaway1.GiveawayContestants.Add(giveawayContestant2);
            giveaway2.GiveawayContestants.Add(giveawayContestant3);

            await _giveawayRepository.AddAsync(giveaway1);
            await _giveawayRepository.AddAsync(giveaway2);
        }

        private async Task SeedDataMatch()
        {
            var pakhus77 = new Club()
            {
                Abbreviation = "P77",
                Name = "Pakhus77",
                Location = "Pakhus77",
                Teams = new List<Team>()
                {

                }
            };

            var team1 = new Team()
            {
                Club = pakhus77,
                Name = "Team 1",
                Players = new List<Player>()
                {
                    new Player() { Name = "Peter" },
                    new Player() { Name = "Jonathan" }
                }
            };
            var team2 = new Team()
            {
                Club = pakhus77,
                Name = "Team 2",
                Players = new List<Player>()
                {
                    new Player() { Name = "Anton" },
                    new Player() { Name = "Oliver" }
                }
            };
            var team3 = new Team()
            {
                Club = pakhus77,
                Name = "Team 3",
                Players = new List<Player>()
                {
                    new Player() { Name = "Louise" },
                    new Player() { Name = "Asbjørn" }
                }
            };

            pakhus77.Teams.Add(team1);
            pakhus77.Teams.Add(team2);
            pakhus77.Teams.Add(team3);

            var københavnPakhus = new Club() { Abbreviation = "KHP", Location = "København ", Name = "København Pakhus" };
            for (int i = 0; i < 6; i++)
            {
                // Makes some opponents
                var opponentPlayers = new List<Player>
                    {
                        new Player { Name = "Player" + (i * 2) },
                        new Player { Name = "Player" + (i * 2 + 1) }
                    };
                Team opponentTeam = new Team { Name = "Opponent", Players = opponentPlayers, Club = københavnPakhus };
                københavnPakhus.Teams.Add(opponentTeam);

                // Makes Score
                // Makes three sets
                var sets = new List<Set>();
                for (int j = 0; j < 3; j++)
                {
                    // Makes 6 games for each
                    var games = new List<Game>();
                    for (int k = 0; k < 6; k++)
                    {
                        games.Add(new Game
                        {
                            Server = false,
                            Number = k,
                            PointHistory = (j % 2 == 0) ? new List<bool> { false, false, false, false, false, false } : new List<bool> { true, true, true, true, true, true }
                        });
                    }
                    sets.Add(new Set
                    {
                        Winner = (j % 2 == 0),
                        Games = games
                    });
                }

                // Makes Score
                var score = new Score { Sets = sets };

                // Makes the match
                var match = new Match
                {
                    Score = score,
                    HomeTeam = pakhus77.Teams[i % 3],
                    AwayTeam = opponentTeam,
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(6 % 2)),
                    Status = Status.Scheduled,
                    Field = i % 3
                };
                await _matchRepository.AddAsync(match);
            }

            int numberOfOngoingMatches = 1;
            for (int i = 0; i < numberOfOngoingMatches; i++)
            {
                // Create some players for the match
                var opponentPlayers = new List<Player>
                    {
                        new Player { Name = "OpponentPlayer" + (i * 2) },
                        new Player { Name = "OpponentPlayer" + (i * 2 + 1) }
                    };
                Team opponentTeam = new Team { Name = "Opponent", Players = opponentPlayers, Club = københavnPakhus };
                københavnPakhus.Teams.Add(opponentTeam);

                // Create sets
                var sets = new List<Set>();

                // Create the match
                var match = new Match
                {
                    HomeTeam = pakhus77.Teams[i],
                    AwayTeam = opponentTeam,
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    Status = Status.Ongoing
                };

                match.Status = Status.Ongoing;
                match.Field = i % 3 + 1;
                var firstSet = new Set();
                firstSet.AddGame(new Game() { Server = true, Number = 0 }); // Count from 0
                match.Score.Sets.Add(firstSet);



                // Add the match to the repository
                await _matchRepository.AddAsync(match);
            }

        }

        private bool dataExists()
        {
            if (_adminUserRepository.GetAllAsync().Result.Count() > 0) return false;
            if (_giveawayRepository.GetAllAsync().Result.Count() > 0) return false;
            if (_matchRepository.GetAllAsync().Result.Count() > 0) return false;

            return true;
        }

        public async Task ResetData()
        {

        }

    }
}



