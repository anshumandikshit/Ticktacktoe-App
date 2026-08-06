using System.Text.Json.Serialization;

namespace API.Models
{
    public class Move
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string Player { get; set; }
        public string Action { get; set; } // e.g. "X at (0,1)"
        public DateTime Timestamp { get; set; }

        [JsonIgnore]
        public Game? Game { get; set; }
    }
}
