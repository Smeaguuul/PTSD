using System.Reflection;

namespace DataAccess.Models
{
    public class Game
    {
        public bool Server { get; set; }
        public int Number { get; }
        private List<bool> _pointHistory;
        public List<bool> PointHistory { get { return [.. _pointHistory]; } }

        public Game(int number)
        {
            Number = number;
            _pointHistory = new List<bool>();
        }

        public Game(bool server, int number, List<bool> pointHistory)
        {
            Server = server;
            Number = number;
            _pointHistory = pointHistory;
        }



        /// <summary>
        /// Adds a winner to the list of points.
        /// </summary>
        public void AddPoint(bool winner)
        {
            if (_pointHistory.Count >= 13) throw new InvalidOperationException("The game can't have any more points.");
            _pointHistory.Add(winner);
        }

        /// <summary>
        /// Removes the last point.
        /// </summary>
        public void RemovePoint()
        {
            if (_pointHistory.Count == 0) throw new InvalidOperationException("There are no points in the list.");
            _pointHistory.RemoveAt(_pointHistory.Count - 1);
        }



    }
}