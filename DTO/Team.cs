using System.Text.Json.Serialization;

namespace DTO
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Player> Players { get; set; }

        [JsonIgnore] // Necessary to prevent self-refence loops
        public Club Club { get; set; }

        public Team() { }
    }
}