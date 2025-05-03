using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Mappers;
using DataAccess.Context;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class MatchesService : IMatchesService
    {
        private readonly IRepository<Match> Matches;
        private readonly IRepository<Team> Teams;
        IMapper Mapper;

        public MatchesService(IMapper mapper, IRepository<Match> repository, IRepository<Team> teamRepository)
        {
            Mapper = mapper;
            Matches = repository;
            Teams = teamRepository;
        }

        public async Task DeleteMatch(int matchId)
        {
            var match = await Matches.FirstOrDefaultAsync(m => m.Id == matchId);
            if (match == null) throw new ArgumentException("Match does not exist.");
            await Matches.RemoveAsync(match);
        }

        public async Task<DTO.MatchScore> GetMatchScore(int matchId)
        {
            var matchScore = new DTO.MatchScore();

            var match = await Matches.FirstOrDefaultAsync(
                predicate: m => m.Id == matchId,
                include: query => query
                .Include(m => m.Score)
                .ThenInclude(s => s.Sets)
                .ThenInclude(s => s.Games)
                .Include(m => m.AwayTeam).ThenInclude(o => o.Players)
                .Include(m => m.AwayTeam).ThenInclude(o => o.Club)
                .Include(m => m.HomeTeam).ThenInclude(h => h.Players)
                .Include(m => m.HomeTeam).ThenInclude(h => h.Club));

            if (match == null) throw new ArgumentException("Match does not exist.");

            if (match.Score.Sets.Count > 0)
            {
                matchScore.SetsHome = match.Score.Sets.Count(set => set.Winner == true);
                matchScore.SetsAway = match.Score.Sets.Count(set => set.Winner == false);


                var lastSet = match.Score.Sets.Last();

                if (lastSet.Games.Count > 0)
                {
                    var lastGame = lastSet.Games.OrderBy(g => g.Number).Last();

                    foreach (var point in lastGame.PointHistory)
                    {
                        if (point)
                        {
                            if (matchScore.PointsHome == "0" || matchScore.PointsHome == null) matchScore.PointsHome = "15";
                            else if (matchScore.PointsHome == "15") matchScore.PointsHome = "30";
                            else if (matchScore.PointsHome == "30") matchScore.PointsHome = "40";
                            else if (matchScore.PointsHome == "40")
                            {
                                if (matchScore.PointsAway == "Game") matchScore.PointsAway = "40";
                                matchScore.PointsHome = "Game";
                            }
                        }
                        else
                        {
                            if (matchScore.PointsAway == "0" || matchScore.PointsAway == null) matchScore.PointsAway = "15";
                            else if (matchScore.PointsAway == "15") matchScore.PointsAway = "30";
                            else if (matchScore.PointsAway == "30") matchScore.PointsAway = "40";
                            else if (matchScore.PointsAway == "40")
                            {
                                if (matchScore.PointsHome == "Game") matchScore.PointsHome = "40";
                                matchScore.PointsAway = "Game";
                            }
                        }
                    }
                    // Removes null values incase no points are awarded to a certain side.
                    if (matchScore.PointsAway == null) matchScore.PointsAway = "0";
                    if (matchScore.PointsHome == null) matchScore.PointsHome = "0";
                    matchScore.GamesThisSetHome = lastSet.Games.Count(game => game.PointHistory.Count > 0 && game.PointHistory.Last() == true && game.Number != lastGame.Number);
                    matchScore.GamesThisSetAway = lastSet.Games.Count(game => game.PointHistory.Count > 0 && game.PointHistory.Last() == false && game.Number != lastGame.Number);
                }
            }

            matchScore.NameAway = match.AwayTeam.Club.Name;
            matchScore.AwayPlayers = [.. match.AwayTeam.Players.Select(p => p.Name)];
            matchScore.NameHome = match.HomeTeam.Club.Name;
            matchScore.HomePlayers = [.. match.HomeTeam.Players.Select(p => p.Name)];
            matchScore.FieldId = match.Field;

            return matchScore;
        }

        public async Task CreateMatch(int homeTeamId, int awayTeamId, DateOnly date, DTO.Status status)
        {
            var homeTeam = await Teams.FirstOrDefaultAsync(t => t.Id == homeTeamId);
            var awayTeam = await Teams.FirstOrDefaultAsync(t => t.Id == awayTeamId);

            // Check if both teams exist
            if (homeTeam == null || awayTeam == null)
            {
                throw new ArgumentException("One or both teams do not exist.");
            }

            var match = new Match
            {
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                Date = date,
                Status = Mapper.Map<Status>(status),
                Score = new Score()
            };

            await Matches.AddAsync(match);
        }

        /// <summary>
        /// Returns all matches that are scheduled and sorts after date.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<DTO.Match>> ScheduledMatches()
        {
            var matches = await Matches.GetAllAsync(
                predicate: m => m.Status == Status.Scheduled,
                include: query => query
                .Include(m => m.Score)
                .ThenInclude(s => s.Sets)
                .ThenInclude(s => s.Games)
                .Include(m => m.AwayTeam).ThenInclude(o => o.Players)
                .Include(m => m.AwayTeam).ThenInclude(o => o.Club)
                .Include(m => m.HomeTeam).ThenInclude(h => h.Players)
                .Include(m => m.HomeTeam).ThenInclude(h => h.Club),
                orderBy: query => query.OrderBy(m => m.Date)
                );
            return Mapper.Map<List<DTO.Match>>(matches);
        }
        /// <summary>
        /// Returns all matches that are ongoing.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<DTO.Match>> OngoingMatches()
        {
            var matches = await Matches.GetAllAsync(
                predicate: m => m.Status == Status.Ongoing,
                include: query => query
                .Include(m => m.Score)
                .ThenInclude(s => s.Sets)
                .ThenInclude(s => s.Games)
                .Include(m => m.AwayTeam).ThenInclude(o => o.Players)
                .Include(m => m.AwayTeam).ThenInclude(o => o.Club)
                .Include(m => m.HomeTeam).ThenInclude(h => h.Players)
                .Include(m => m.HomeTeam).ThenInclude(h => h.Club)
                );
            return Mapper.Map<List<DTO.Match>>(matches);
        }

        /// <summary>
        /// Starts the match, and creates the first set and game.
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="server"></param>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task StartMatch(int matchId, bool server, int fieldId)
        {
            // Gets the match and updates the status and field
            var match = await Matches.FirstOrDefaultAsync(m => m.Id == matchId, query => query.Include(m => m.Score).ThenInclude(s => s.Sets).ThenInclude(s => s.Games));
            if (match == null) throw new ArgumentException("Match does not exist!");
            if (match.Status != Status.Scheduled || match.Score.Sets.Count != 0) throw new ArgumentException("Match already ongoing!");
            match.Status = Status.Ongoing;
            match.Field = fieldId;

            var firstSet = new Set();
            firstSet.AddGame(new Game() { Server = server, Number = 0 }); // Count from 0
            match.Score.Sets.Add(firstSet);

            await Matches.UpdateAndSaveAsync(match);
        }

        /// <summary>
        /// Updates the match score according to the pointWinner given.
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="pointWinner">True means homeTeam, False means awayTeam</param>
        /// <returns></returns>
        public async Task UpdateMatchScore(int matchId, bool pointWinner)
        {
            // Gets the match and fetches the latest game
            var match = await Matches.FirstOrDefaultAsync(m => m.Id == matchId, query => query.Include(m => m.Score).ThenInclude(s => s.Sets).ThenInclude(s => s.Games));
            if (match == null) throw new ArgumentException("Match does not exist!");

            // Gets the current set and game
            var latestSet = match.Score.Sets.OrderByDescending(set => set.Id).ToList()[0]; // Gets the set with the highest ID == Last to get created.
            var latestGame = latestSet.Games.OrderByDescending(game => game.Number).ToList()[0]; // Same principle, but with our given number.
            latestGame.PointHistory.Add(pointWinner);

            // Gets the won games for each team this set
            var homeTeamWonGamesThisSet = latestSet.Games.Count(game => game.PointHistory.Last() == true && game.Number != latestGame.Number);
            var awayTeamWonGamesThisSet = latestSet.Games.Count(game => game.PointHistory.Last() == false && game.Number != latestGame.Number);

            // TODO Check for tie break
            var tieBreak = homeTeamWonGamesThisSet == 6 && awayTeamWonGamesThisSet == 6;

            // Gets the score
            var score = new GameScore();

            if (tieBreak)
            {
                foreach (var point in latestGame.PointHistory)
                {
                    if (point)
                    {
                        // Checks if hometeam won
                        if (score.homePoints == 6 && score.awayPoints <= 5) score.homePoints++;
                        // Checks if awayteam had advantage
                        else if (score.awayPoints == 6)
                        {
                            score.homePoints++;
                            score.awayPoints--;
                        }
                        //Else it is just regular point going to home.
                        else score.homePoints++;
                    }
                    if (!point)
                    {
                        // Checks if awayteam won
                        if (score.awayPoints == 6 && score.homePoints <= 5) score.awayPoints++;
                        // Checks if hometeam had advantage
                        else if (score.homePoints == 6)
                        {
                            score.awayPoints++;
                            score.homePoints--;
                        }
                        //Else it is just regular point going to away.
                        else score.awayPoints++;
                    }
                }

                // Home won
                if (score.homePoints == 7)
                {
                    latestSet.Winner = true;
                }
                // Away won
                if (score.awayPoints == 7)
                {
                    latestSet.Winner = false;
                }
            }
            else
            {
                foreach (var point in latestGame.PointHistory)
                {
                    CalculatePoints(point, ref score); // Passes reference
                }

                // Checks if a teams has won
                if (score.homePoints == 5) // Hometeam win
                {
                    // Checks if the hometeam has won this set
                    if (homeTeamWonGamesThisSet == 5 && awayTeamWonGamesThisSet < 5)
                    {
                        latestSet.Winner = true;
                    }
                    // If it hasn't won the set, we just start a new game
                    else
                    {
                        // Switches server, and gives the new game a number one higher than the current game
                        latestSet.Games.Add(new Game() { Server = !latestGame.Server, Number = latestGame.Number + 1 });
                    }
                }
                else if (score.awayPoints == 5) // Awayteam win
                {
                    // Checks if the awayTeam has won this set
                    if (awayTeamWonGamesThisSet == 5 && homeTeamWonGamesThisSet < 5)
                    {
                        latestSet.Winner = false;
                    }
                    // If it hasn't won the set, we just start a new game
                    else
                    {
                        // Switches server, and gives the new game a number one higher than the current game
                        latestSet.Games.Add(new Game() { Server = !latestGame.Server, Number = latestGame.Number + 1 });
                    }
                }
            }

            // Checks if the set is over
            if (latestSet.Winner != null)
            {
                // Checks if the match is not over
                if (!(match.Score.Sets.Count(s => s.Winner == true) == 2 || match.Score.Sets.Count(s => s.Winner == false) == 2))
                {
                    // The set is done, but not the match, there we prepare a new set.
                    var newSet = new Set();
                    newSet.Games.Add(new Game() { Server = latestGame.Server, Number = 20 });
                    match.Score.Sets.Add(newSet);
                }
            }
            await Matches.UpdateAndSaveAsync(match);
        }

        private static void CalculatePoints(bool pointWinner, ref GameScore score)
        {
            // Checks different scenarios

            // Checks for deuce win
            if (score.homePoints == 4 && score.awayPoints == 3)
            {
                if (!pointWinner)
                {
                    score.homePoints--;
                    score.awayPoints++;
                }
                else score.homePoints++;
            }
            else if (score.awayPoints == 4 && score.homePoints == 3)
            {
                if (pointWinner)
                {
                    score.homePoints++;
                    score.awayPoints--;
                }
                else score.awayPoints++;
            }

            //Checks for deuce
            else if (score.homePoints == 3 && score.awayPoints == 3)
            {
                if (pointWinner) score.homePoints++;
                if (!pointWinner) score.awayPoints++;

            }

            // Checks for win
            else if (score.homePoints == 3 && score.awayPoints < 3)
            {
                score.homePoints++;
            }
            else if (score.awayPoints == 3 && score.homePoints < 3)
            {
                score.awayPoints++;
            }

            // If none of those, just add points to team
            else if (pointWinner) score.homePoints++;
            else if (!pointWinner) score.awayPoints++;
        }

        internal struct GameScore
        {
            /// <summary>
            /// 0 = 0, 1 = 15, 2 = 30, 3 = 40, 4 = game, 5 = won
            /// </summary>
            public int homePoints;
            /// <summary>
            /// 0 = 0, 1 = 15, 2 = 30, 3 = 40, 4 = game, 5 = won.
            /// </summary
            public int awayPoints;

            public GameScore()
            {
                homePoints = 0;
                awayPoints = 0;
            }
        }

        /// <summary>
        /// Seed data for scheduled games. Creates 6 matches with 3 sets each, and 6 games per set.
        /// </summary>
        /// <returns></returns>
        public async Task SeedMatchData()
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
                await Matches.AddAsync(match);
            }

            for (int i = 0; i < 3; i++) // Only create 3 ongoing games
            {
                // Create some players for the match
                var opponentPlayers = new List<Player>
                    {
                        new Player { Name = "OpponentPlayer" + (i * 2) },
                        new Player { Name = "OpponentPlayer" + (i * 2 + 1) }
                    };
                Team opponentTeam = new Team { Name = "Opponent", Players = opponentPlayers, Club = københavnPakhus };
                københavnPakhus.Teams.Add(opponentTeam);

                // Determine the number of sets (1 or 2)
                int numberOfSets = new Random().Next(1, 3); // Randomly choose between 1 and 2 sets

                // Create sets
                var sets = new List<Set>();
                for (int j = 0; j < numberOfSets; j++)
                {
                    // Create games for each set
                    var games = new List<Game>();
                    for (int k = 0; k < 6; k++) // Assuming 6 games per set
                    {
                        games.Add(new Game
                        {
                            Server = (k % 2 == 0), // Alternate server for demonstration
                            Number = k,
                            PointHistory = new List<bool> { true, false, true, false, true, false } // Example point history
                        });
                    }
                    sets.Add(new Set
                    {
                        Winner = (j % 2 == 0), // Alternate winner for demonstration
                        Games = games
                    });
                }

                // Create the score
                var score = new Score { Sets = sets };

                // Create the match
                var match = new Match
                {
                    Score = score,
                    HomeTeam = pakhus77.Teams[i],
                    AwayTeam = opponentTeam,
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    Status = Status.Ongoing,
                    Field = i // Assign to one of the three fields
                };

                // Add the match to the repository
                await Matches.AddAsync(match);
            }
        }
    }
}
