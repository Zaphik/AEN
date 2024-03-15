using System.Collections.Generic;
using Panda.MGModel;
using Panda.MGModel.SRC.GamePlay;

// The health gauge for the hero
public sealed class HealthGauge : Gauge
{
    // The heart sprite placed next to the health gauge
    private readonly BaseSprite Heart;

    public HealthGauge(GameServiceContainer SERVICES) : base("2D/Misc/Foreground.PNG",
        new Vector2(40, GameState.ScreenHeight - 65), new Vector2(124, 15), SERVICES)
    {
        // Sets the current and maximum size of the gauge and sends the health to the gauge
        User.SendHealth = HEALTH =>
        {
            var health = (List<float>)HEALTH;
            currSize = health[0];
            maxSize = health[1];
        };
        Heart = new BaseSprite("2D/Misc/heart.png", new Vector2(15, GameState.ScreenHeight - 55), new Vector2(25, 25),
            SERVICES);
    }

    public override void Update()
    {
        Heart.Update();
        base.Update();
    }

    public override void Draw(GameServiceContainer SERVICES)
    {
        Heart.Draw(Vector2.Zero, SERVICES);
        base.Draw(SERVICES);
    }
}