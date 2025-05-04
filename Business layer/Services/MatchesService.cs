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

            match = MatchScoreController.UpdateScore(match, pointWinner);
            
            await Matches.UpdateAndSaveAsync(match);
        }

        public async Task UndoMatchPoint(int matchId)
        {
            var match = await Matches.FirstOrDefaultAsync(m => m.Id == matchId, query => query.Include(m => m.Score).ThenInclude(s => s.Sets).ThenInclude(s => s.Games));
            if (match == null) throw new ArgumentException("Match does not exist!");

            match = MatchScoreController.UndoPoint(match);

            await Matches.UpdateAndSaveAsync(match);
        }


        public async Task<DTO.MatchScore> GetMatchScore(int matchId)
        {
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

            return MatchScoreController.MapMatch(match);
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
