using System.Collections.Generic;
using Panda.MGModel.Projectiles;
using Panda.MGModel.SRC.Engine;
using Panda.MGModel.SRC.GamePlay;

namespace Panda.MGModel;

public class Wizard : Building
{
    // The range of the wizard
    private readonly int range;


    // The amount of spawns and the max amount of spawns
    private readonly int maxSpawns;
    private int currSpawns { get; set; }

    // The distance of the nearest mob and the current distance
    private float nearestDist { get; set; }
    private float currentDist { get; set; }

    // The nearest mob
    private Mob? nearest { get; set; }

    // The spawn timer
    private readonly Timer shotTimer;

    // The location of the wizard
    private readonly GridLoc loc;

    public Wizard(Vector2 POS, Grid GRID, GameServiceContainer SERVICES) : base("2D/Builds/Charry.png", POS,
        new Vector2(15, 15) * GameState.Settings.ScreenRatio, 1, 1, 1000, SERVICES)
    {
        // gets the location of the wizard
        loc = GRID.GetLocFromRelPos(GRID.GetRelPosFromPos(pos - new Vector2(0, 50)));

        // if the location is not filled it fills it
        if (loc is { IsFilled: false })
        {
            loc.Fill(true);

            // sets the position of the wizard to the center of the location
            pos = GRID.GetPosFromLoc(GRID.GetRelPosFromPos(pos - new Vector2(0, 50)));
        }

        // sets the range, current spawns and max spawns
        range = 360;
        currSpawns = 0;
        maxSpawns = 4;

        // sets the shot timer
        shotTimer = new Timer(700);

        // sets the health of the wizard
        currHealth = 10;
        maxHealth = currHealth;

        // sets the hit distance of the wizard
        hitDist = 35.0f;
    }

    public override void Update(Vector2 OFFSET, List<Mob?> mobs)
    {
        // updates the shot timer and if it is not ready it returns
        shotTimer.UpdateTimer();
        if (!shotTimer.Test()) return;

        // invokes the wizard and resets the shot timer
        Invoke(mobs);
        shotTimer.ResetToZero();

        base.Update(OFFSET);
    }

    private void Invoke(IReadOnlyList<Mob?> mobs)
    {
        // if the current spawns is less than the max spawns it finds the nearest mob and adds a projectile
        if (currSpawns < maxSpawns)
        {
            nearestDist = range;
            nearest = null;

            for (var i = mobs.Count - 1; i >= 0; i--)
            {
                currentDist = GetDist(mobs[i]!.pos, pos);

                if (!(currentDist < nearestDist)) continue;
                nearestDist = currentDist;
                nearest = mobs[i];
            }

            if (null == nearest) return;
            World.AddProjectile?.Invoke(new Arrow(pos, this, nearest.pos, Services));

            // increments the current spawns
            currSpawns++;
        }
        else
        {
            // if the current spawns is equal to the max, it unfills the location and sets the wizard to dead
            Dead = true;
            loc.Fill(false);
        }
    }

    public override void Draw(Vector2 OFFSET, GameServiceContainer SERVICES)
    {
        base.Draw(OFFSET, SERVICES);
    }
}