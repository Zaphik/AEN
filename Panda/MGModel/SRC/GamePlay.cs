using System;
using Microsoft.Xna.Framework.Content;
using Panda.MGModel.SRC.Engine;
using Panda.MGModel.SRC.GamePlay;

namespace Panda.MGModel;

// Container for the UI and the world
public class GamePlay
{
    private World world { get; set; }
    private UI ui { get; set; }

    // Timer for the win condition
    private readonly Timer winTimer;

    public static Action ResetWorld { get; set; }

    private GameServiceContainer Services { get; set; }

    public GamePlay(GameServiceContainer SERVICES)
    {
        // Sets the world and the UI
        world = new World(SERVICES);
        ui = new UI(world.user.hero, SERVICES);

        // Sets the win timer to 60 seconds
        winTimer = new Timer(60000);

        // Sets the win condition to false
        GameState.IsWin = false;

        // Resets the world
        ResetWorld = () =>
        {
            world = new World(SERVICES);
            ui = new UI(world.user.hero, SERVICES);
            winTimer.ResetToZero();
            GameState.IsWin = false;
        };

        Services = SERVICES;
    }

    public void Update()
    {
        // Updates the wintimer if the game isn't paused
        if (!GameState.IsPaused)
        {
            // updates the timer and sets win to true if the time has been reached
            winTimer.UpdateTimer();
            if (winTimer.Test()) GameState.IsWin = true;
        }


        // Updates the world if the win condition hasn't been reached
        if (!GameState.IsWin) world.Update();

        // TOggles between the state of the world if escape is pressed
        if (Services.GetService<Keyboard>().GetSinglePress("Escape")) GameState.IsPaused = !GameState.IsPaused;
        ui.Update();
    }

    public void Draw(GameServiceContainer SERVICES)
    {
        world.Draw(SERVICES);
        ui.Draw(SERVICES);
    }
}