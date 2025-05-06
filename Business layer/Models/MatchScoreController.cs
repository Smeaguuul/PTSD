using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;

namespace Business.Models
{
    internal static class MatchScoreController
    {
        public static DTO.MatchScore MapMatch(Match match)
        {
            var matchScore = new DTO.MatchScore();

            if (match.Score.Sets.Count > 0)
            {
                matchScore.SetsHome = match.Score.Sets.Count(set => set.Winner == true);
                matchScore.SetsAway = match.Score.Sets.Count(set => set.Winner == false);


                var lastSet = match.Score.Sets.OrderByDescending(set => set.Id).ToList()[0];

                if (lastSet.Games.Count > 0)
                {
                    var lastGame = lastSet.Games.OrderBy(g => g.Number).Last();

                    // Get's won games this set for each team
                    matchScore.GamesThisSetHome = GetWonGamesThisSet(lastSet, true, lastGame.Number);
                    matchScore.GamesThisSetAway = GetWonGamesThisSet(lastSet, false, lastGame.Number);

                    var tieBreak = matchScore.GamesThisSetHome == 6 && matchScore.GamesThisSetAway == 6;
                    if (tieBreak)
                    {
                        matchScore.PointsHome = lastGame.PointHistory.Count(p => p == true).ToString();
                        matchScore.PointsAway = lastGame.PointHistory.Count(p => p == false).ToString();
                    }
                    else
                    {
                        CalculatePoints(matchScore, lastGame);
                    }
                    // Removes null values incase no points are awarded to a certain side.
                    matchScore.PointsAway ??= "0";
                    matchScore.PointsHome ??= "0";


                }
            }

            matchScore.NameAway = match.AwayTeam.Club.Name;
            matchScore.AwayPlayers = [.. match.AwayTeam.Players.Select(p => p.Name)];
            matchScore.NameHome = match.HomeTeam.Club.Name;
            matchScore.HomePlayers = [.. match.HomeTeam.Players.Select(p => p.Name)];
            matchScore.FieldId = match.Field;

            return matchScore;
        }

        private static void CalculatePoints(DTO.MatchScore matchScore, Game lastGame)
        {
            foreach (var point in lastGame.PointHistory)
            {
                if (point)
                {
                    if (matchScore.PointsHome == "0" || matchScore.PointsHome == null) matchScore.PointsHome = "15";
                    else if (matchScore.PointsHome == "15") matchScore.PointsHome = "30";
                    else if (matchScore.PointsHome == "30") matchScore.PointsHome = "40";
                    else if (matchScore.PointsHome == "40")
                    {
                        if (matchScore.PointsAway == "Advantage") matchScore.PointsAway = "40";
                        matchScore.PointsHome = "Advantage";
                    }
                }
                else
                {
                    if (matchScore.PointsAway == "0" || matchScore.PointsAway == null) matchScore.PointsAway = "15";
                    else if (matchScore.PointsAway == "15") matchScore.PointsAway = "30";
                    else if (matchScore.PointsAway == "30") matchScore.PointsAway = "40";
                    else if (matchScore.PointsAway == "40")
                    {
                        if (matchScore.PointsHome == "Advantage") matchScore.PointsHome = "40";
                        matchScore.PointsAway = "Advantage";
                    }
                }
            }
        }

        public static Match UndoPoint(Match match)
        {
            // Gets the current set and game
            var latestSet = match.Score.Sets.OrderByDescending(set => set.Id).ToList()[0]; // Gets the set with the highest ID == Last to get created.
            var latestGame = latestSet.Games.OrderByDescending(game => game.Number).ToList()[0]; // Same principle, but with our given number.

            // No points have been played = do nothing
            if (latestGame.PointHistory.Count == 0 && latestSet.Games.Count == 1 && match.Score.Sets.Count == 1) return match;
            // New game started in new set
            else if (latestGame.PointHistory.Count == 0 && latestSet.Games.Count == 1 && match.Score.Sets.Count > 1)
            {
                match.Score.Sets.Remove(latestSet);
                latestSet = match.Score.Sets.OrderByDescending(set => set.Id).ToList()[0];
                latestSet.Winner = null;
                latestGame = latestSet.Games.OrderByDescending(game => game.Number).ToList()[0];
                latestGame.PointHistory.RemoveAt(latestGame.PointHistory.Count - 1);  // Can be assumed not to be empty
            }
            // New game started in same set
            else if (latestGame.PointHistory.Count == 0 && latestSet.Games.Count > 1 && match.Score.Sets.Count > 1)
            {
                latestSet.Games.Remove(latestGame);
                latestGame = latestSet.Games.OrderByDescending(game => game.Number).ToList()[0];
                latestGame.PointHistory.RemoveAt(latestGame.PointHistory.Count - 1); // Can be assumed not to be empty
            }
            // Just a point in a continuing game
            else if (latestGame.PointHistory.Count > 0)
            {
                latestGame.PointHistory.RemoveAt(latestGame.PointHistory.Count - 1);  // Can be assumed not to be empty
            }

            return match;
        }

        public static Match UpdateScore(Match match, bool pointWinner)
        {
            // Gets the current set and game
            var latestSet = match.Score.Sets.OrderByDescending(set => set.Id).ToList()[0]; // Gets the set with the highest ID == Last to get created.
            var latestGame = latestSet.Games.OrderByDescending(game => game.Number).ToList()[0]; // Same principle, but with our given number.
            latestGame.PointHistory.Add(pointWinner);

            // Gets the won games for each team this set
            var homeTeamWonGamesThisSet = GetWonGamesThisSet(latestSet, true, latestGame.Number);
            var awayTeamWonGamesThisSet = GetWonGamesThisSet(latestSet, false, latestGame.Number);

            // Check for tie break
            var tieBreak = homeTeamWonGamesThisSet == 6 && awayTeamWonGamesThisSet == 6;

            var score = new GameScore();

            if (tieBreak)
            {
                TieBreakerPointCalculator(latestSet, latestGame, ref score);
                // Home won
                if (score.homePoints == "Won")
                {
                    latestSet.Winner = true;
                }
                // Away won
                if (score.awayPoints == "Won")
                {
                    latestSet.Winner = false;
                }
            }
            else
            {
                foreach (var point in latestGame.PointHistory)
                {
                    CalculatePoints(point, ref score); // Passes reference
                }

                // Checks if a team has won
                if (score.homePoints == "Won")
                {
                    if (homeTeamWonGamesThisSet == 5 && awayTeamWonGamesThisSet < 5) latestSet.Winner = true;
                    // If it hasn't won the set, we just start a new game
                    else latestSet.Games.Add(new Game() { Server = !latestGame.Server, Number = latestGame.Number + 1 });
                }
                else if (score.awayPoints == "Won")
                {
                    if (awayTeamWonGamesThisSet == 5 && homeTeamWonGamesThisSet < 5) latestSet.Winner = false;
                    // If it hasn't won the set, we just start a new game
                    else latestSet.Games.Add(new Game() { Server = !latestGame.Server, Number = latestGame.Number + 1 });
                }
            }
            // Checks if the set is over
            if (latestSet.Winner != null)
            {
                // Checks if the match is not over
                if (!(match.Score.Sets.Count(s => s.Winner == true) == 2 || match.Score.Sets.Count(s => s.Winner == false) == 2))
                {
                    // The set is done, but not the match, therefor we prepare a new set.
                    var newSet = new Set();
                    newSet.Games.Add(new Game() { Server = latestGame.Server, Number = 20 });
                    match.Score.Sets.Add(newSet);
                } else match.Status = Status.Finished;
            }
            return match;
        }

        private static void CalculatePoints(bool homeWins, ref GameScore score)
        {
            // Checks different scenarios

            // Checks for deuce win
            if (score.homePoints == "Advantage" && score.awayPoints == "40")
            {
                if (!homeWins)
                {
                    score.homePoints = "40";
                    score.awayPoints = "Advantage";
                }
                else score.homePoints = "Won";
            }
            else if (score.awayPoints == "Advantage" && score.homePoints == "40")
            {
                if (homeWins)
                {
                    score.homePoints = "Advantage";
                    score.awayPoints = "40";
                }
                else score.awayPoints = "Won";
            }

            //Checks for deuce
            else if (score.homePoints == "40" && score.awayPoints == "40")
            {
                if (homeWins) score.homePoints = "Advantage";
                else score.awayPoints = "Advantage";

            }

            // Checks for win
            else if (score.homePoints == "40" && score.awayPoints != "40" && homeWins) // Assumption: awayPoints can't be "Advantage" here
            {
                score.homePoints = "Won";
            }
            else if (score.awayPoints == "40" && score.homePoints != "40" && !homeWins)
            {
                score.awayPoints = "Won";
            }

            // If none of those, just add points to team
            else if (homeWins)
            {
                if (score.homePoints == "0") score.homePoints = "15";
                else if (score.homePoints == "15") score.homePoints = "30";
                else if (score.homePoints == "30") score.homePoints = "40";
            }
            else if (!homeWins)
            {
                if (score.awayPoints == "0") score.awayPoints = "15";
                else if (score.awayPoints == "15") score.awayPoints = "30";
                else if (score.awayPoints == "30") score.awayPoints = "40";
            }
        }

        private static void TieBreakerPointCalculator(Set latestSet, Game latestGame, ref GameScore score)
        {
            var homePoints = 0;
            var awayPoints = 0;

            foreach (var homeWin in latestGame.PointHistory)
            {
                if (homeWin) homePoints++;
                else awayPoints++;

                if (homePoints >= 7 && homePoints - awayPoints >= 2) score.homePoints = "Won";
                if (awayPoints >= 7 && awayPoints - homePoints >= 2) score.awayPoints = "Won";
            }
        }

        private static int GetWonGamesThisSet(Set latestSet, bool homeTeam, int latestGameNumber)
        {
            return latestSet.Games.Count(game => game.PointHistory.Count > 0 && game.PointHistory.Last() == homeTeam && game.Number != latestGameNumber);
        }

        internal struct GameScore
        {
            /// <summary>
            /// 0 = 0, 1 = 15, 2 = 30, 3 = 40, 4 = advantage, 5 = won
            /// </summary>
            public string homePoints;
            /// <summary>
            /// 0 = 0, 1 = 15, 2 = 30, 3 = 40, 4 = advantage, 5 = won.
            /// </summary
            public string awayPoints;

            public GameScore()
            {
                homePoints = "0";
                awayPoints = "0";
            }
        }
    }
}
