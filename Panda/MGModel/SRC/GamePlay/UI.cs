using System.Collections.Generic;
using Panda.MGModel.Buttons;

namespace Panda.MGModel.SRC.GamePlay;

// Stores all the UI elements
public class UI
{
    private readonly ScoreCounter scoreCounter;

    private readonly HealthGauge healthGauge;
    private readonly TimerGauge timerGauge;

    private readonly Death death;
    private readonly Hero hero;

    private readonly List<BaseButton> Buttons;

    public UI(Hero HERO, GameServiceContainer SERVICES)
    {
        death = new Death(SERVICES);
        scoreCounter = new ScoreCounter(SERVICES);
        hero = HERO;
        healthGauge = new HealthGauge(SERVICES);
        timerGauge = new TimerGauge(SERVICES);

        Buttons =
        [
            new PauseBtn(SERVICES),
            new ExitBtn(SERVICES),
            new SaveBtn(hero, SERVICES)
        ];
    }

    public void Update()
    {
        healthGauge.Update();
        scoreCounter.Update();
        timerGauge.Update();

        for (var i = Buttons.Count - 1; i >= 0; i--) Buttons[i].Update();
    }

    public void Draw(GameServiceContainer SERVICES)
    {
        // if the hero is dead draw the death screen
        if (hero.Dead)
        {
            death.Draw(SERVICES);
        }
        else
        {
            // Else draw the UI elements
            scoreCounter.Draw(SERVICES);
            healthGauge.Draw(SERVICES);
            timerGauge.Draw(SERVICES);
        }

        // if the game is not paused return
        if (!GameState.IsPaused) return;

        // Draw the buttons
        for (var i = Buttons.Count - 1; i >= 0; i--) Buttons[i].Draw(SERVICES);
    }
}