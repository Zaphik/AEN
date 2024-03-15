using System;
using System.Collections.Generic;
using Panda.MGModel.SRC.Engine;
using Panda.MGModel.SRC.GamePlay;

namespace Panda.MGModel;

public class Hero : Unit
{
    // bool to check if the hero is moving
    private bool isMoving;

    public bool IsMoving
    {
        get => isMoving;
        private set => isMoving = value;
    }

    // nearest mob to the hero
    private Mob? nearest { get; set; }

    // range of the hero
    private readonly int range;

    // nearest distance to the hero and the current distance
    private float nearestDist { get; set; }
    private float currentDist { get; set; }

    // timer for the hero to shoot
    private readonly Timer shootTimer;

    protected GameServiceContainer Services { get; set; }

    protected Hero(string PATH, Vector2 POS, Vector2 SIZE, int ROWS, int COLUMNS, int TICK,
        GameServiceContainer SERVICES) : base(PATH, POS, SIZE, ROWS, COLUMNS, TICK, SERVICES)
    {
        Services = SERVICES;
        // sets the values of the hero
        range = 470;

        currHealth = 10;
        maxHealth = currHealth;
        rot = 0;

        shootTimer = new Timer(500);
    }


    public virtual void Update(Vector2 OFFSET, Grid GRID, List<Mob?> MOBS)
    {
        // sets is moving to false
        IsMoving = false;

        // moves the hero
        Move(GRID);
        Invoke(MOBS);

        base.Update(OFFSET);
    }

    public virtual void Invoke(List<Mob?> MOBS)
    {
        // updates the shoot timer and returns if the timer is not done
        shootTimer.UpdateTimer();
        if (!shootTimer.Test()) return;

        // sets the nearestdist to the range and the nearest to null
        nearestDist = range;
        nearest = null;

        // cycles through each mob
        for (var i = MOBS.Count - 1; i >= 0; i--)
        {
            // updates currentdist and checks if it is less than the nearestdist
            currentDist = GetDist(MOBS[i]!.pos, pos);

            // if it is not less than the nearestdist it continues
            if (!(currentDist < nearestDist)) continue;

            // sets the nearest distance to the current distance and the nearest to the current mob
            nearestDist = currentDist;
            nearest = MOBS[i];
        }

        // if the nearest is null it returns else it adds a projectile
        if (null == nearest) return;
        World.AddProjectile?.Invoke(CreateProjectile(nearest.pos));

        // resets the shoot timer
        shootTimer.ResetToZero();
    }

    // returns a new projectile
    protected virtual BaseProjectile CreateProjectile(Vector2 TARGET)
    {
        throw new NotImplementedException();
    }

    private void Move(Grid GRID)
    {
        // sets the next position to the current position
        var nextPosX = pos.X;
        var nextPosY = pos.Y;

        // if the hero is moving within bounds and is not in contact with a building it moves
        if (pos.X > -100 * GameState.Settings.ScreenRatio &&
            Services.GetService<Keyboard>().GetPress(GameState.Settings.Left))
        {
            nextPosX -= velo;

            // flips the sprite
            SpriteFX = SpriteEffects.FlipHorizontally;
        }

        if (pos.X < GameState.ScreenWidth + 100 * GameState.Settings.ScreenRatio &&
            Services.GetService<Keyboard>().GetPress(GameState.Settings.Right))
        {
            nextPosX += velo;

            // resets the sprite
            SpriteFX = SpriteEffects.None;
        }

        if (Services.GetService<Keyboard>().GetPress(GameState.Settings.Down) &&
            pos.Y < GameState.ScreenHeight + 1500 * GameState.Settings.ScreenRatio) nextPosY += velo;

        if (Services.GetService<Keyboard>().GetPress(GameState.Settings.Up) &&
            pos.Y > -1500 * GameState.Settings.ScreenRatio) nextPosY -= velo;

        var nextGridLocX = GRID.GetLocFromPos(new Vector2(nextPosX, pos.Y));
        if (nextGridLocX is { IsFilled: false })
        {
            pos = pos with { X = nextPosX };
            IsMoving = true;
        }

        var nextGridLocY = GRID.GetLocFromPos(new Vector2(pos.X, nextPosY));
        if (nextGridLocY is { IsFilled: false })
        {
            pos = pos with { Y = nextPosY };
            IsMoving = true;
        }


        // if the hero is moving it checks if the screen should scroll
        if (IsMoving) World.CheckScroll?.Invoke();


        // adds a wizard either under it or above it
        if (Services.GetService<Keyboard>().GetSinglePress(GameState.Settings.Build))
            User.AddBuilding?.Invoke(Services.GetService<Keyboard>().GetPress(GameState.Settings.Up)
                ? new Wizard(pos + new Vector2(0, 100), GRID, Services)
                : new Wizard(pos, GRID, Services));
    }

    public override void Draw(Vector2 OFFSET, GameServiceContainer SERVICES)
    {
        base.Draw(OFFSET, SERVICES);
    }
}