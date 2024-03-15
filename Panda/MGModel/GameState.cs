using Panda.Server;

namespace Panda.MGModel;

// Ok hear me out
// Sure, every single thing is static.
// But when it comes to a game, you gotta have some state and there comes a point where Dependency Injection is not the answer
// That's coming from the guy that's passed services into almost every class
// Thus a class specific for state was born
public abstract class GameState
{
    // The state of the game
    public static bool IsPaused { get; set; }
    public static bool IsWin { get; set; }


    //The settings, save and score
    public static Settings? Settings { get; set; }
    public static Save? Save { get; set; }
    public static Score? Score { get; set; }

    //The screen size
    public static int ScreenWidth { get; set; }
    public static int ScreenHeight { get; set; }
}