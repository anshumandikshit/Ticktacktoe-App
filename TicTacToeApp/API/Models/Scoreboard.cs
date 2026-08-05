namespace API.Models
{
    public class Scoreboard
    {
        public int Id { get; set; }
        public string Player { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
    }

}
