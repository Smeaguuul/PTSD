using DTO;
namespace Presentation.Models
{
    public class MatchViewModel
    {
        public Match Match { get; set; }
        public List<(int TeamOneScore, int TeamTwoScore)> SetScores { get; set; }
    }
}
