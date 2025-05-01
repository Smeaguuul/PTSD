
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

        public void AddSet(Set set) //Remove needed? - Mikkel
        {
            if (set == null) throw new ArgumentNullException(nameof(set), "Set cannot be null.");

            Sets.Add(set);
        }
    }
}