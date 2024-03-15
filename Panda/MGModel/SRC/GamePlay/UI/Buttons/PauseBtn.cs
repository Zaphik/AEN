namespace Panda.MGModel.Buttons;

// The pause button
public class PauseBtn : BaseButton
{
    public PauseBtn(GameServiceContainer SERVICES) : base("2D/UI/Play.png",
        new Vector2(GameState.ScreenWidth / 2, GameState.ScreenHeight / 2 - 75 * GameState.Settings.ScreenRatio),
        new Vector2(75, 75) * GameState.Settings.ScreenRatio, SERVICES)
    {
    }


    public override void Update()
    {
        base.Update();
    }

    // Unpauses the game
    protected override void Invoke()
    {
        GameState.IsPaused = false;
        base.Invoke();
    }

    public override void Draw(GameServiceContainer SERVICES)
    {
        base.Draw(SERVICES);
    }
}