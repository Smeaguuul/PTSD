using DTO;

namespace Presentation.Models
{
    public class Matches
    {
        public List<MatchScore> MatchScores { get; set; } = new List<MatchScore>();
    }
    public class MatchScore
    {
        public string NameAway { get; set; }
        public string NameHome { get; set; }
        public int SetsHome { get; set; }
        public int SetsAway { get; set; }
        public int GamesThisSetHome { get; set; }
        public int GamesThisSetAway { get; set; }
        public int PointsHome { get; set; }
        public int PointsAway { get; set; }
        public int FieldId { get; set; }

        public static MatchScore ConvertMatchToMatchScore(Match match)
        {
            var matchScore = new MatchScore();

            if (match.Score != null && match.Score.Sets != null)
            {
                matchScore.SetsHome = match.Score.Sets.Count(set => set.Winner);
                matchScore.SetsAway = match.Score.Sets.Count(set => !set.Winner);

                if (match.Score.Sets.Count > 0)
                {
                    var lastSet = match.Score.Sets.Last();

                    if (lastSet.Games.Count > 0)
                    {
                        var lastGame = lastSet.Games.Last();
                        matchScore.PointsHome = lastGame.PointHistory.Count(p => p);
                        matchScore.PointsAway = lastGame.PointHistory.Count(p => !p);
                    }
                }
            }

            matchScore.NameAway = match.AwayTeam.Club.Name;
            matchScore.NameHome = match.HomeTeam.Club.Name;
            matchScore.FieldId = match.Field.Id;

            return matchScore;
        }
    }
}
