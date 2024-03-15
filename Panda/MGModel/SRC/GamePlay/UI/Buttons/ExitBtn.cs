namespace Panda.MGModel.Buttons;

// The exit button
public class ExitBtn : BaseButton
{
    public ExitBtn(GameServiceContainer SERVICES) : base("2D/UI/Exit.png",
        new Vector2(GameState.ScreenWidth / 2, GameState.ScreenHeight / 2),
        new Vector2(75, 75) * GameState.Settings.ScreenRatio, SERVICES)
    {
    }

    public override void Update()
    {
        base.Update();
    }

    // Exits the game
    protected override void Invoke()
    {
        Pandemonium.ExitGame.Invoke();

        base.Invoke();
    }

    public override void Draw(GameServiceContainer SERVICES)
    {
        base.Draw(SERVICES);
    }
}