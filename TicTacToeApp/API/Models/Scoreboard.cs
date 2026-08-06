namespace API.Models
{
    public class Scoreboard
    {
        public int Id { get; set; }              // Primary key
        public Guid SessionId { get; set; }    // Unique per session
        public int XWins { get; set; }           // Total wins by X
        public int OWins { get; set; }           // Total wins by O
        public int Draws { get; set; }           // Total draws
    }

}
