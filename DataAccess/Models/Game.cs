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


    }
}