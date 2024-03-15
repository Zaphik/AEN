using System;
using System.Collections.Generic;

namespace Panda.MGModel.SRC.GamePlay;

public sealed class User
{
    public static Action<List<float>> SendHealth { get; set; }

    // The hero
    public readonly Hero hero;

    // The list of wizards the hero places
    private readonly List<Building> buildings;

    public static Action<Building> AddBuilding { get; private set; }

    public User(GameServiceContainer SERVICES)
    {
        // Sets the hero to the choice of the user
        hero = GameState.Save?.Choice switch
        {
            "Erza" => new Erza(SERVICES),
            _ => new Natsu(SERVICES)
        };
        // Sets the list of buildings to an empty list
        buildings = [];

        // Adds the building to the game
        AddBuilding = BUILDING => buildings.Add(BUILDING);
    }

    // Updates the user
    public void Update(Vector2 OFFSET, List<Mob?> mobs, Grid GRID)
    {
        hero.Update(OFFSET, GRID, mobs);

        for (var i = buildings.Count - 1; i >= 0; i--)
        {
            buildings[i].Update(OFFSET, mobs);
            if (!buildings[i].Dead) continue;
            buildings.RemoveAt(i);
            i--;
        }

        // Sends the health of the hero to the HealthGauge
        SendHealth?.Invoke([hero.currHealth, hero.maxHealth]);
    }


    // Draws the user
    public void Draw(Vector2 OFFSET, GameServiceContainer SERVICES)
    {
        hero.Draw(OFFSET, SERVICES);

        for (var i = buildings.Count - 1; i >= 0; i--) buildings[i].Draw(OFFSET, SERVICES);
    }
}