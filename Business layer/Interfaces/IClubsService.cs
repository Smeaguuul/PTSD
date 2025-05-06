using DTO;

namespace Business.Interfaces
{
    public interface IClubsService
    {
        Task AddTeamToClub(string TeamName, string ClubAbbreviation, string Player1Name, string Player2Name, string clubAbbreviation);
        Task CreateClub(string Name, string Abbreviation, string Location);
        Task<IEnumerable<Club>> GetAll();
        Task RemoveTeamFromClub(int teamId, string clubAbbreviation);
    }
}