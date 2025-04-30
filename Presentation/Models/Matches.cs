namespace Presentation.Models
{
    public class Matches
    {
        public List<string> Scores { get; set; }

        public Matches( List<string> scores)
        {
            this.Scores = scores;
        }
    }
}
