
namespace DataAccess.Models
{
    public class Score
    {
        public int Id { get; set; }

        public List<Set> sets; // TODO: Consider maybe array of length 3 - Mikkel
        public List<Set> Sets { get { return sets; } }

        public Score(int id, List<Set> sets)
        {
            Id = id;
            this.sets = sets;
        }

        public Score()
        {
            sets = new List<Set>();
        }

        public void AddSet(Set set) //Remove needed? - Mikkel
        {
            if (set == null) throw new ArgumentNullException(nameof(set), "Set cannot be null.");

            sets.Add(set);
        }
    }
}