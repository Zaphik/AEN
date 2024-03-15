namespace Panda.Server;

// Score model that replicate the database table
public class Score
{
    public int? ScoreID { get; init; }
    public int HighScore { get; set; }
    public int LongestLived { get; set; }
    public int? UserID { get; init; }
    public string Character { get; set; }
    public string Username { get; set; }
}