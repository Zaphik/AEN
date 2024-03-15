namespace Panda.Server;

// User model that replicate the database table
public class User
{
    public int UserID { get; init; }
    public string Username { get; init; }
    public string HashedPassword { get; set; }
    public string Question { get; init; }
    public string HashedAnswer { get; set; }
}