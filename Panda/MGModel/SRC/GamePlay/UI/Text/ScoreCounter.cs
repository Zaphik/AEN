using System;
using Panda.MGModel.Buttons;

namespace Panda.MGModel;

public class ScoreCounter
{
    // The amount of kills
    private int kills { get; set; }

    // The font and the text
    private readonly SpriteFont font;
    private string text { get; set; }

    public static Action<int> SendKills { get; private set; }

    public ScoreCounter(GameServiceContainer SERVICES)
    {
        // Loads the font and sets the kills
        font = SERVICES.GetService<ContentManager>().Load<SpriteFont>("Fonts/Peanut Butter");
        kills = 0;

        // Sends the kills to the savebtn
        SendKills = KILLS => kills = KILLS;

        // gets the score
        SaveBtn.GetScore = () => kills * 1000;
    }


    public void Update()
    {
        // Updates the text
        text = "Score:" + kills * 1000;
    }

    public void Draw(GameServiceContainer SERVICES)
    {
        // Draws the text
        SERVICES.GetService<SpriteBatch>().DrawString(font, text,
            new Vector2(GameState.ScreenWidth / 2 - font.MeasureString(text).X / 2, GameState.ScreenHeight - 100),
            Color.Black);
    }
}