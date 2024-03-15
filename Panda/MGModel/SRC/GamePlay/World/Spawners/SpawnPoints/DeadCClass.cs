using Panda.MGModel.SRC.GamePlay;

namespace Panda.MGModel.SpawnPoints;

public class DeadCClass : SpawnPoint
{
    // the max amount of mobs that can be spawned
    private readonly int maxSpawns;
    private int currSpawns { get; set; }


    public DeadCClass(Vector2 POS, GameServiceContainer SERVICES) : base("2D/Units/Mobs/DeadCClass.png", POS,
        new Vector2(25, 25) * GameState.Settings.ScreenRatio, 1, 1, 1000, SERVICES)
    {
        // sets the spawn time, current spawns and max spawns
        spawnTime = 800;
        currSpawns = 0;
        maxSpawns = 3;
    }

    public override void Update(Vector2 OFFSET)
    {
        base.Update(OFFSET);
    }


    protected override void Invoke()
    {
        // spawns a mob and increments the current spawns
        if (currSpawns < maxSpawns)
        {
            World.AddMob?.Invoke(new BClass(new Vector2(pos.X, pos.Y), Services));
            currSpawns++;
        }
        else
        {
            // if the max spawns is reached the spawn point is dead
            Dead = true;
        }
    }

    public override void Draw(Vector2 OFFSET, GameServiceContainer SERVICES)
    {
        base.Draw(OFFSET, SERVICES);
    }
}