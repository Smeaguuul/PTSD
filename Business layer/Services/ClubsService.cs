using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class ClubsService : IClubsService
    {
        private readonly IRepository<Club> Clubs;
        IMapper Mapper;

        public ClubsService(IMapper mapper, IRepository<Club> repository)
        {
            Mapper = mapper;
            Clubs = repository;
        }

        public async Task<IEnumerable<DTO.Club>> GetAll()
        {
            var clubs = await Clubs.GetAllAsync(
                include: query => query
                .Include(c => c.Teams).ThenInclude(t => t.Players)
                );
            return Mapper.Map<List<DTO.Club>>(clubs);
        }

        public async Task CreateClub(string Name, string Abbreviation, string Location)
        {
            var Club = new Club() { Name = Name, Abbreviation = Abbreviation, Location = Location, Teams = [] };
            await Clubs.AddAsync(Club);
        }

        public async Task AddTeamToClub(string TeamName, string ClubAbbreviation, string Player1Name, string Player2Name, string clubAbbreviation)
        {
            var club = await Clubs.FirstOrDefaultAsync(c => c.Abbreviation.Equals(clubAbbreviation));
            if (club == null) throw new ArgumentException("Club doesn't exist.");
            var playerList = new List<Player>();
            playerList.Add(new Player() { Name = Player1Name });
            playerList.Add(new Player() { Name = Player2Name });
            var team = new Team() { Club = club, Name = TeamName, Players = playerList };
            club.Teams.Add(Mapper.Map<Team>(team));
            await Clubs.UpdateAndSaveAsync(club);
        }

        public async Task RemoveTeamFromClub(int teamId, string clubAbbreviation)
        {
            var club = await Clubs.FirstOrDefaultAsync(c => c.Abbreviation.Equals(clubAbbreviation));
            if (club == null) throw new ArgumentException("Club doesn't exist.");
            if (club.Teams.Count(t => t.Id == teamId) == 0) throw new ArgumentException($"The club doesn't have team with the id of {teamId}.");
            club.Teams.RemoveAll(t => t.Id == teamId);
            await Clubs.UpdateAndSaveAsync(club);
        }
    }
}
