namespace Panda.MGModel;

public class Death
{
    // The font and the text
    private readonly SpriteFont font;
    private readonly string text;

    public Death(GameServiceContainer SERVICES)
    {
        // Loads the font and sets the text
        font = SERVICES.GetService<ContentManager>().Load<SpriteFont>("Fonts/Peanut Butter");
        text = $"You Died, Press {GameState.Settings.Reset} to Restart";
    }

    public void Draw(GameServiceContainer SERVICES)
    {
        // Draws the text
        SERVICES.GetService<SpriteBatch>().DrawString(font, text,
            new Vector2(GameState.ScreenWidth / 2 - font.MeasureString(text).X / 2, GameState.ScreenHeight / 2),
            Color.DarkBlue);
    }
}