using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Mappers;
using Business.Models;
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

        public async Task<DTO.Match> GetMatch(int matchId)
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
            if (match == null) throw new ArgumentException("Match does not exist!");

            return Mapper.Map<DTO.Match>(match);
        }
        public async Task<IEnumerable<DTO.Match>> GetTodaysMatchesWithScore() {
            var matches = await Matches.GetAllAsync(
                predicate: m => m.Date == DateOnly.FromDateTime(DateTime.Now),
                include: query => query
                .Include(m => m.Score)
                .ThenInclude(s => s.Sets));
            return Mapper.Map<List<DTO.Match>>(matches);
        }
        

        /// <summary>
        /// Returns all matches that are finished and sorts after date.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<DTO.Match>> FinishedMatches()
        {
            var matches = await Matches.GetAllAsync(
                predicate: m => m.Status == Status.Finished,
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
            if (match.Status != Status.Scheduled) throw new ArgumentException("Match already ongoing!");
            match.Status = Status.Ongoing;
            match.Field = fieldId;
            var firstSet = new Set();
            firstSet.AddGame(new Game() { Server = server, Number = 0 }); // Count from 0
            match.Score.Sets.Add(firstSet);

            await Matches.UpdateAndSaveAsync(match);
        }
        /// <summary>
        /// Ends the match.
        /// </summary>
        /// <param name="matchId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task EndMatch(int matchId)
        {
            var match = await Matches.FirstOrDefaultAsync(m => m.Id == matchId, query => query.Include(m => m.Score).ThenInclude(s => s.Sets).ThenInclude(s => s.Games));
            if (match == null) throw new ArgumentException("Match does not exist!");
            if (match.Status != Status.Ongoing) throw new ArgumentException("Match not ongoing!" + match.Status);
            match.Status = Status.Finished;
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

        public async Task ChangeFinishedGameScore(int matchId, int setsHome, int setsAway)
        {
            if (setsHome < 0 || setsAway < 0) throw new ArgumentException("Sets cannot be negative.");
            if (setsHome > 2 || setsAway > 2) throw new ArgumentException("Sets won cannot be above 2.");
            if (setsHome + setsAway > 3) throw new ArgumentException("Number of sets cannot be more that 3.");

            var match = await Matches.FirstOrDefaultAsync(m => m.Id == matchId, query => query.Include(m => m.Score).ThenInclude(s => s.Sets).ThenInclude(s => s.Games));
            if (match == null) throw new ArgumentException("Match does not exist!");

            // Check if the match is already finished
            if (match.Status != Status.Finished) throw new ArgumentException("Match is not finished.");

            var score = match.Score;
            match.Score.Sets.Clear();
            for (int i = 0; i < setsHome; i++)
            {
                var set = new Set() { Winner = true };
                score.Sets.Add(set);
            }
            for (int i = 0; i < setsAway; i++)
            {
                var set = new Set() { Winner = false };
                score.Sets.Add(set);
            }

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
    }
}
