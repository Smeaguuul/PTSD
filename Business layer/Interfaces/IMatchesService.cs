using DTO;

namespace Business.Interfaces
{
    public interface IMatchesService
    {
        Task ChangeFinishedGameScore(int matchId, int setsHome, int setsAway);
        Task CreateMatch(int homeTeamId, int awayTeamId, DateOnly date, Status status);
        Task DeleteMatch(int matchId);
        Task EndMatch(int matchId);
        Task<IEnumerable<Match>> FinishedMatches();
        Task<Match> GetMatch(int matchId);
        Task<MatchScore> GetMatchScore(int matchId);
        Task<IEnumerable<Match>> OngoingMatches();
        Task<IEnumerable<Match>> ScheduledMatches();
        Task SeedMatchData();
        Task StartMatch(int matchId, bool server, int fieldId);
        Task UndoMatchPoint(int matchId);
        Task UpdateMatchScore(int matchId, bool pointWinner);
    }
}