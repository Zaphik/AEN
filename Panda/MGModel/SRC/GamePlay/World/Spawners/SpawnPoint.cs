using Panda.MGModel.SRC.Engine;
using Panda.MGModel.SRC.GamePlay;

namespace Panda.MGModel;

public class SpawnPoint : AtkObject
{
    // The spawn timer and the spawn time
    private readonly Timer spawnTimer;
    protected int spawnTime { get; set; }

    private protected GameServiceContainer Services { get; set; }

    public SpawnPoint(string PATH, Vector2 POS, Vector2 SIZE, int ROWS, int COLUMNS, int TICK,
        GameServiceContainer SERVICES) : base(PATH, POS, SIZE, ROWS, COLUMNS, TICK, SERVICES)
    {
        Services = SERVICES;
        // Sets the spawn time and the spawn timer
        spawnTime = 2200;
        spawnTimer = new Timer(spawnTime);

        // Sets the health of the spawn point
        currHealth = 3;
        maxHealth = currHealth;
    }

    public override void Update(Vector2 OFFSET)
    {
        // Updates the spawn timer and checks if it is time to spawn
        spawnTimer.UpdateTimer();

        if (spawnTimer.Test())
        {
            // Spawns a mob
            Invoke();

            // Resets the spawn timer
            spawnTimer.ResetToZero();
        }

        base.Update(OFFSET);
    }

    // Spawns a mob
    protected virtual void Invoke()
    {
        World.AddMob?.Invoke(new DClass(new Vector2(pos.X, pos.Y + 33), Services));
    }

    public override void Draw(Vector2 OFFSET, GameServiceContainer SERVICES)
    {
        base.Draw(OFFSET, SERVICES);
    }
}