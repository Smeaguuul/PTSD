using System.Reflection;

namespace Business.Models
{
    public class Game
    {
        private bool _server;
        public bool Server { get { return _server; } set { _server = value; } }
        public readonly int _number;
        private List<bool> _pointHistory;

        public Game(int number)
        {
            _number = number;
            _pointHistory = new List<bool>();
        }

        /// <summary>
        /// Adds a winner to the list of points.
        /// </summary>
        public void addPoint(bool winner)
        {
            if (_pointHistory.Count >= 13) throw new Exception("The game can't have any more points.");
            _pointHistory.Add(winner);
        }
        /// <summary>
        /// Removes the last Point.
        /// </summary>
        public void removePoint()
        {
            if (_pointHistory.Count == 0) throw new Exception("There are no points in the list.");
            _pointHistory.RemoveAt(_pointHistory.Count - 1);
        }


    }
}