using System;
using Panda.MGModel.SRC.Engine;

namespace Panda.MGModel;

public class MobSpawner
{
    // the three spawners
    private readonly DSpawner dSpawner;
    private readonly CSpawner cSpawner;
    private readonly ASpawner aSpawner;

    // the spawn timer and the random number
    private readonly Timer spawnTimer;
    private int rand { get; set; }

    public MobSpawner(int MSEC, GameServiceContainer SERVICES)
    {
        // sets the spawn timer and the random number
        spawnTimer = new Timer(MSEC);

        rand = new Random().Next(1, 31);

        //Instantiates the spawners
        cSpawner = new CSpawner(SERVICES);
        dSpawner = new DSpawner(SERVICES);
        aSpawner = new ASpawner(SERVICES);
    }

    public void Update(Vector2 OFFSET, Grid GRID)
    {
        // Updates the spawn timer and checks if it is time to spawn
        spawnTimer.UpdateTimer();
        if (!spawnTimer.Test()) return;

        // Randomly spawn a mob
        switch (rand)
        {
            case < 16:
                dSpawner.Update(OFFSET, GRID);
                break;
            case < 26:
                cSpawner.Update(OFFSET, GRID);
                break;
            default:
                aSpawner.Update(OFFSET, GRID);
                break;
        }

        // Resets the spawn timer and the random number
        spawnTimer.ResetToZero();
        rand = new Random().Next(1, 31);
    }
}