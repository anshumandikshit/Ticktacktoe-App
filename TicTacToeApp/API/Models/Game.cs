namespace API.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        public string CurrentTurn { get; set; } // Player1 / Player2 / Computer
        public string Status { get; set; } // Active, Completed, Reset
        public ICollection<Move> Moves { get; set; }
    }
}