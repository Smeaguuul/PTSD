namespace Business.Interfaces
{
    public interface IMatchesService
    {
        Task<IEnumerable<DTO.Match>> ScheduledMatches();

    }
}