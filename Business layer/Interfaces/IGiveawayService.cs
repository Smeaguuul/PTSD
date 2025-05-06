using DataAccess.Models.Giveaways;
using DTO.Giveaway;

namespace Business.Interfaces
{
    public interface IGiveawayService
    {
        Task<bool> AddContestantToGiveawayAsync(int giveawayId, string email, string name);
        Task<Giveaway> CreateGiveawayAsync(CreateGiveawayDto giveawayDto);
        Task<Giveaway> CreateGiveawayAsync(string name, string description, DateTime start, DateTime end);
        Task DeleteGiveaway(int giveawayID);
        Task<IEnumerable<ContestantDto>> GetContestants(int giveawayId);
        Task<IEnumerable<GiveawayDto>> GetGiveaways();
        Task<ContestantDto> PickWinner(int giveawayId);
        Task<IEnumerable<ContestantDto>> PickWinner(int amountOfWinners, int giveawayId);
        Task<bool> RemoveContestantFromGiveawayAsync(int giveawayId, int contestantId);
        
    }
}