using System.Collections.Generic;
using Panda.MGModel.SRC.Engine;

namespace Panda.MGModel;

public class BaseProjectile : AnimatedSprite
{
    private bool done;

    public bool Done
    {
        get => done;
        private set => done = value;
    }

    // The direction of the projectile
    private readonly Vector2 dir;

    // The velocity of the projectile
    protected float velo;

    // The timer of the projectile
    private readonly Timer timer;

    // The sender of the projectile
    private readonly AtkObject sender;

    public BaseProjectile(string PATH, Vector2 POS, Vector2 SIZE, AtkObject SENDER, Vector2 TARGET, int ROWS,
        int COLUMNS, int TICK, GameServiceContainer SERVICES) : base(PATH, POS, SIZE, ROWS, COLUMNS, TICK, SERVICES)
    {
        // Sets the values of the projectile
        Done = false;

        velo = 2.5f * GameState.Settings.ScreenRatio;


        sender = SENDER;

        dir = TARGET - SENDER.pos;
        dir.Normalize();

        timer = new Timer(1200);

        // rotates the projectile towards the target
        rot = RotateTowards(pos, new Vector2(TARGET.X, TARGET.Y));
    }


    public void Update(List<Unit> UNITS)
    {
        // Moves the projectile
        pos += dir * velo;

        // Updates the timer and checks if the projectile is done
        timer.UpdateTimer();
        if (timer.Test()) Done = true;

        if (Collision(UNITS)) Done = true;
        base.Update();
    }

    private bool Collision(IReadOnlyList<Unit> UNITS)
    {
        // cycles through all the units  and checks if the projectile has collided with any of them
        for (var i = UNITS.Count - 1; i >= 0; i--)
        {
            if (UNITS[i] == sender) continue;
            if (!(GetDist(pos, UNITS[i].pos) < UNITS[i].hitDist)) continue;
            UNITS[i].GetHit(1);
            return true;
        }

        return false;
    }

    public override void Draw(Vector2 OFFSET, GameServiceContainer SERVICES)
    {
        base.Draw(OFFSET, SERVICES);
    }
}