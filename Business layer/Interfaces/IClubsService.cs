using DTO;

namespace Business.Interfaces
{
    public interface IClubsService
    {
        Task AddTeamToClub(string TeamName, string Player1Name, string Player2Name, string ClubAbbreviation);
        Task CreateClub(string Name, string Abbreviation, string Location);
        Task<IEnumerable<Club>> GetAll();
        Task RemoveTeamFromClub(int teamId, string clubAbbreviation);
    }
}