namespace Business.Models
{
    internal class Player
    {
        private readonly int _id;
        public int Id { get { return _id; } }
        private readonly string _name;
        public string Name { get { return _name; } }

        public Player(int id, string name)
        {
            _id = id;
            _name = name;
        }
    }
}