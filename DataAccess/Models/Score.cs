
namespace DataAccess.Models
{
    public class Score
    {
        public int Id { get; set; }

        public List<Set> Sets { get; set; }

        public Score(int id, List<Set> sets)
        {
            Id = id;
            this.Sets = sets;
        }

        public Score()
        {
            Sets = new List<Set>();
        }
    }
}