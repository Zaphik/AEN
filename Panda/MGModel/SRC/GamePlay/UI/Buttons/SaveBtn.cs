using System;
using Panda.Services.DbCTRL;

namespace Panda.MGModel.Buttons;

// The save button
public class SaveBtn : BaseButton
{
    private readonly Hero Hero;

    public static Func<int> GetScore { get; set; }
    public static Func<int> GetTimePlayed { get; set; }

    public SaveBtn(Hero HERO, GameServiceContainer SERVICES) : base("2D/UI/Save.png",
        new Vector2(GameState.ScreenWidth / 2, GameState.ScreenHeight / 2 + 75 * GameState.Settings.ScreenRatio),
        new Vector2(75, 75) * GameState.Settings.ScreenRatio, SERVICES)
    {
        Hero = HERO;
    }


    public override void Update()
    {
        base.Update();
    }

    // Saves the game
    protected override async void Invoke()
    {
        base.Invoke();

        // gets the score and time played
        var score = GetScore();
        var time = GetTimePlayed();

        // if the user is logged in and the game is won or the hero is dead
        if (UiGlobals.UserID == null || (!GameState.IsWin && !Hero.Dead)) return;
        // if the score is not null it updates the score
        if (GameState.Score != null)
            await SqlCTRL.UpdateScore((int)GameState.Score.ScoreID!, score, time, GameState.Save?.Choice);

        // if the save is not null it updates the save
        if (GameState.Save != null) await SqlCTRL.UpdateSave((int)GameState.Save.SaveID!);
    }

    public override void Draw(GameServiceContainer SERVICES)
    {
        base.Draw(SERVICES);
    }
}