using System;
using System.Collections.Generic;
using System.Linq;

namespace Panda.MGModel.SRC.GamePlay;

public class World
{
    // The user
    public readonly User user;

    // The number of mobs killed
    private int numKilled { get; set; }

    // The list of mobs and the mob spawner
    private readonly List<Mob?> mobs;
    private readonly MobSpawner mobSpawner;

    // The list of spawn points
    private readonly List<SpawnPoint> spawnPoints;

    // The list of projectiles
    private readonly List<BaseProjectile> projectiles;

    // The offset of the world to set the camera
    private Vector2 offset;

    // The grid and the tilemap
    private readonly Grid grid;
    private readonly TileMap tileMap;


    public static Action<Mob> AddMob { get; private set; }
    public static Action<BaseProjectile> AddProjectile { get; private set; }
    public static Action CheckScroll { get; set; }


    public GameServiceContainer Services { get; set; }

    public World(GameServiceContainer SERVICES)
    {
        // Sets the state to not paused
        GameState.IsPaused = false;

        //Sets the values of the world
        user = new User(SERVICES);

        spawnPoints = [];
        projectiles = [];
        mobs = [];

        mobSpawner = new MobSpawner(550, SERVICES);
        offset = Vector2.Zero;

        grid = new Grid(new Vector2(25, 25) * GameState.Settings.ScreenRatio,
            new Vector2(-100, -1500) * GameState.Settings.ScreenRatio,
            new Vector2(GameState.ScreenWidth + 200, GameState.ScreenHeight + 3000) * GameState.Settings.ScreenRatio);
        tileMap = new TileMap(grid, SERVICES);


        // Adds the mobs, spawn points, and projectiles to the game
        AddMob = MOB => mobs.Add(MOB);
        AddProjectile = PROJECTILE => projectiles.Add(PROJECTILE);

        CClass.AddSpawnPoint = SPAWNPOINT => spawnPoints.Add(SPAWNPOINT);

        // Checks if the world should scroll
        CheckScroll = Scroll;

        Services = SERVICES;
    }

    public void Update()
    {
        // if the user is not dead and the game is not paused, it updates the world
        if (!user.hero.Dead && !GameState.IsPaused)
        {
            user.Update(offset, mobs, grid);


            for (var i = mobs.Count - 1; i >= 0; i--)
            {
                mobs[i]?.Update(offset, user.hero, grid);
                if (!mobs[i]!.Dead) continue;
                if (mobs[i]!.Killed)
                {
                    numKilled++;
                    ScoreCounter.SendKills?.Invoke(numKilled);
                }

                mobs.RemoveAt(i);
                i--;
            }

            mobSpawner.Update(offset, grid);

            for (var i = spawnPoints.Count - 1; i >= 0; i--)
            {
                spawnPoints[i].Update(offset);
                if (!spawnPoints[i].Dead) continue;
                spawnPoints.RemoveAt(i);
                i--;
            }

            for (var i = projectiles.Count - 1; i >= 0; i--)
            {
                var units = new List<Unit> { user.hero };
                units.AddRange(mobs.Cast<Unit>().ToList());
                projectiles[i].Update(units);

                if (!projectiles[i].Done) continue;
                projectiles.RemoveAt(i);
                i--;
            }
        }
        else
        {
            // if the user presses the reset button, it resets the world
            if (Services.GetService<Keyboard>().GetPress(GameState.Settings?.Reset))
                MGModel.GamePlay.ResetWorld?.Invoke();
        }
    }

    private void Scroll()
    {
        if (user.hero.pos.X < -offset.X + GameState.ScreenWidth / 5 && offset.X < 100 * GameState.Settings.ScreenRatio)
            // A
            offset.X += user.hero.velo;

        if (user.hero.pos.X > -offset.X + GameState.ScreenWidth - GameState.ScreenWidth / 5 &&
            offset.X > -100 * GameState.Settings.ScreenRatio)
            // D
            offset.X -= user.hero.velo;

        if (user.hero.pos.Y < -offset.Y + GameState.ScreenHeight / 5 &&
            offset.Y < 1500 * GameState.Settings.ScreenRatio)
            // W
            offset.Y += user.hero.velo;

        if (user.hero.pos.Y > -offset.Y + GameState.ScreenHeight - GameState.ScreenHeight / 5 &&
            offset.Y > -1500 * GameState.Settings.ScreenRatio)
            // S
            offset.Y -= user.hero.velo;
    }


    // Draws the world
    public void Draw(GameServiceContainer SERVICES)
    {
        tileMap.Draw(offset, SERVICES);

        user.Draw(offset, SERVICES);
        for (var i = mobs.Count - 1; i >= 0; i--) mobs[i].Draw(offset, SERVICES);

        for (var i = spawnPoints.Count - 1; i >= 0; i--) spawnPoints[i].Draw(offset, SERVICES);
        for (var i = projectiles.Count - 1; i >= 0; i--) projectiles[i].Draw(offset, SERVICES);
    }
}