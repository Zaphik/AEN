using System;
using Panda.MGModel.SRC.GamePlay;

namespace Panda.MGModel;

// The spawner for the DClass mob
public class DSpawner : ISpawner
{
    // The side the mob will spawn from
    public int Side { get; set; }

    // The position of the mob on the vertical and horizontal side
    public int VertexSidePos { get; set; }
    public int HorizonSidePos { get; set; }


    public GameServiceContainer Services { get; set; }

    public DSpawner(GameServiceContainer SERVICES)
    {
        Services = SERVICES;
        // sets the side of the mob
        Side = new Random().Next(1, 5);
    }


    public void Update(Vector2 OFFSET, Grid grid)
    {
        // spawns the mob and sets the side and position of the mob
        SpawnMob(OFFSET, grid);
        Side = new Random().Next(1, 5);
        VertexSidePos = new Random().Next(0, GameState.ScreenHeight);
        HorizonSidePos = new Random().Next(0, GameState.ScreenWidth);
    }

    public void SpawnMob(Vector2 OFFSET, Grid grid)
    {
        // sets the spawn position of the mob depending on the side
        Vector2 spawnPosition;
        switch (Side)
        {
            case 1:
                spawnPosition = new Vector2(VertexSidePos - OFFSET.X, -OFFSET.Y);
                break;
            case 2:
                spawnPosition = new Vector2(VertexSidePos - OFFSET.X, GameState.ScreenHeight - OFFSET.Y);
                break;
            case 3:
                spawnPosition = new Vector2(-OFFSET.X, HorizonSidePos - OFFSET.Y);
                break;
            case 4:
                spawnPosition = new Vector2(GameState.ScreenWidth - OFFSET.X, HorizonSidePos - OFFSET.Y);
                break;
            default:
                return;
        }

        // Checks if the grid location is filled
        var gridLoc = grid.GetLocFromPos(spawnPosition);
        if (gridLoc is { IsFilled: false }) World.AddMob?.Invoke(new DClass(spawnPosition, Services));
    }
}