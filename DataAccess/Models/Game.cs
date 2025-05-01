using System.Reflection;

namespace DataAccess.Models
{
    public class Game
    {
        public int Id { get; set; }
        public bool Server { get; set; }
        public int Number { get; set; }
        public List<bool> PointHistory { get; set; } = new List<bool>(); // Initialize to avoid null reference

        // Parameterless constructor
        public Game() { }

        
        /// <summary>
        /// Adds a winner to the list of points.
        /// </summary>
        public void AddPoint(bool winner)
        {
            if (PointHistory.Count >= 13) throw new InvalidOperationException("The game can't have any more points.");
            PointHistory.Add(winner);
        }

        /// <summary>
        /// Removes the last point.
        /// </summary>
        public void RemovePoint()
        {
            if (PointHistory.Count == 0) throw new InvalidOperationException("There are no points in the list.");
            PointHistory.RemoveAt(PointHistory.Count - 1);
        }



    }
}