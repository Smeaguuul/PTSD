using DTO;

namespace Presentation.Models
{
    public class AddMatchModel
    {
        public int Id { get; set; }
        public int SelectedHomeTeamId { get; set; }
        public int SelectedAwayTeamId { get; set; }
        public DateOnly Date { get; set; }
        public Status Status { get; set; }

        public List<Club> Clubs { get; set; }
        public List<Team> Teams { get; set; }
        public List<Field> Fields { get; set; }
    }
}
