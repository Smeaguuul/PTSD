
namespace DataAccess.Models
{
    public class Score
    {
        public int Id { get; set; }

        private List<Set> _sets; // TODO: Consider maybe array of length 3 - Mikkel
        public List<Set> Sets { get { return _sets; } }

        public Score(int id, List<Set> sets)
        {
            Id = id;
            _sets = sets;
        }

        public Score()
        {
            _sets = new List<Set>();
        }

        public void AddSet(Set set) //Remove needed? - Mikkel
        {
            if (set == null) throw new ArgumentNullException(nameof(set), "Set cannot be null.");

            _sets.Add(set);
        }
    }
}