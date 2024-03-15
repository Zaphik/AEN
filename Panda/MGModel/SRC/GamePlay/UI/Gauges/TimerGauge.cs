using System;
using Panda.MGModel.Buttons;
using Panda.MGModel.SRC.Engine;

namespace Panda.MGModel;

// The timer gauge for the game
public class TimerGauge : Gauge
{
    // the amount of time that has elapsed
    private double elapsedTime { get; set; }

    // The timer sprite placed next to the timer gauge
    private readonly BaseSprite GaugeTimer;

    public TimerGauge(GameServiceContainer SERVICES) : base("2D/Misc/Foreground.png",
        new Vector2(GameState.ScreenWidth / 2 - 124, 10), new Vector2(248, 30), SERVICES)
    {
        GaugeTimer = new BaseSprite("2D/Misc/clock.png", new Vector2(GameState.ScreenWidth / 2 - 140, 25),
            new Vector2(25, 25), SERVICES);

        // Sets the current and maximum size of the gauge
        maxSize = 60000;
        currSize = maxSize;

        // Sets the elapsed time to 0
        elapsedTime = 0;

        // Sends the time played to the saventb
        SaveBtn.GetTimePlayed = () => (int)elapsedTime / 1000;
    }

    public override void Update()
    {
        // Only updates the timer if the game is not paused
        if (!GameState.IsPaused)
        {
            // Updates the elapsed time
            elapsedTime += Timer.gameTime.ElapsedGameTime.TotalMilliseconds;

            // Updates currSize to be proportional to the remaining time
            currSize = Math.Max(0, maxSize - (float)elapsedTime);
        }

        GaugeTimer.Update();
        base.Update();
    }

    public override void Draw(GameServiceContainer SERVICES)
    {
        GaugeTimer.Draw(Vector2.Zero, SERVICES);
        base.Draw(SERVICES);
    }
}