using System;
using System.Collections.Generic;
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
        private readonly IRepository<Match> Repository;
        IMapper Mapper;

        public MatchesService(IMapper mapper, IRepository<Match> repository)
        {
            Mapper = mapper;
            Repository = repository;
        }

        public async Task<IEnumerable<DTO.Match>> ScheduledMatches()
        {
            var matches = await Repository.GetAllAsync(
                predicate: m => m.Status == Status.Scheduled,
                include: query => query
                .Include(m => m.Score)
                .ThenInclude(s => s.Sets)
                .ThenInclude(s => s.Games)
                .Include(m => m.Opponent).ThenInclude(o=>o.Players),
                orderBy: query => query.OrderBy(m => m.Date)
                );
            return Mapper.Map<List<DTO.Match>>(matches);
        }
        public async Task<IEnumerable<DTO.Match>> OngoingMatches()
        {
            var matches = await Repository.GetAllAsync(
                predicate: m => m.Status == Status.Ongoing,
                include: query => query
                .Include(m => m.Score)
                .ThenInclude(s => s.Sets)
                .ThenInclude(s => s.Games)
                .Include(m => m.Opponent)
                );
            return Mapper.Map<List<DTO.Match>>(matches);
        }

        public async Task ScheduledGamesSeedData()
        {
            for (int i = 0; i < 6; i++)
            {
                // Makes some opponents
                var opponentPlayers = new List<Player>
                    {
                        new Player { Name = "Player" + (i * 2) },
                        new Player { Name = "Player" + (i * 2 + 1) }
                    };
                Team opponentTeam = new Team { Name = "Opponent", Players = opponentPlayers };

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
                    Opponent = opponentTeam,
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(6 % 2)),
                    Status = Status.Scheduled,
                    Field = i % 3
                };
                await Repository.AddAsync(match);
            }
        }
        public async Task OngoingMatchesSeedData()
        {
            for (int i = 0; i < 3; i++) // Only create 3 ongoing games
            {
                // Create some players for the match
                var opponentPlayers = new List<Player>
                    {
                        new Player { Name = "OpponentPlayer" + (i * 2) },
                        new Player { Name = "OpponentPlayer" + (i * 2 + 1) }
                    };
                Team opponentTeam = new Team { Name = "Opponent", Players = opponentPlayers };

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
                    Opponent = opponentTeam,
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(i)), // Assign a different date for each match
                    Status = Status.Ongoing,
                    Field = i // Assign to one of the three fields
                };

                // Add the match to the repository
                await Repository.AddAsync(match);
            }
        }
    }
}
