using Panda.MGModel.SRC.Engine;

namespace Panda.MGModel;

public class Mob : Unit
{
    // the range of the mob
    protected float range { get; init; }

    // despawn timer for the mob
    private readonly Timer despawnTimer;

    protected Mob(string PATH, Vector2 POS, Vector2 SIZE, int ROWS, int COLUMNS, int TICK,
        GameServiceContainer SERVICES) : base(PATH, POS, SIZE, ROWS, COLUMNS, TICK, SERVICES)
    {
        // sets the values of the mob
        velo = 2.0f;
        range = 50.0f;
        despawnTimer = new Timer(12000);
    }

    public virtual void Update(Vector2 OFFSET, Hero HERO, Grid GRID)
    {
        // checks if the mob is dead
        if (!Dead)
        {
            // runs the AI of the mob and updates the despawn timer
            AI(HERO, GRID);
            despawnTimer.UpdateTimer();

            // checks if the mob should despawn
            if (despawnTimer.Test())
            {
                Dead = true;
                despawnTimer.ResetToZero();
            }
        }


        base.Update(OFFSET);
    }

    public virtual void AI(Hero HERO, Grid GRID)
    {
        // checks if the hero is in range
        if (GetDist(pos, HERO.pos) <= 5)
        {
            // hits the hero and decreases the health of the mob
            HERO.GetHit(1);
            currHealth--;

            // if the health is less than or equal to 0 the mob is dead
            if (currHealth <= 0) Dead = true;
        }
        else
        {
            // Moves towards the hero
            pos += RsmbMovement(HERO.pos, pos, velo);
            rot = RotateTowards(pos, HERO.pos);
        }
    }

    public override void GetHit(float DAMAGE)
    {
        base.GetHit(DAMAGE);
    }

    public override void Draw(Vector2 OFFSET, GameServiceContainer SERVICES)
    {
        base.Draw(OFFSET, SERVICES);
    }
}