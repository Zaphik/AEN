using System.Collections.Generic;
using System.Linq;
using Panda.MGModel.Projectiles;
using Panda.MGModel.SRC.Engine;
using Panda.MGModel.SRC.GamePlay;

namespace Panda.MGModel;

public class AClass : Mob
{
    // Timer for the attack
    private readonly Timer attackTimer;

    // Bool to check if the mob is attacking
    private bool attacking { get; set; }


    private List<Vector2>? PathNodes { get; set; }

    // The next node of the mob
    private Vector2 NextNode { get; set; }

    // The last position of the hero
    private Vector2 lastHeroPos { get; set; }


    private GameServiceContainer Services { get; set; }

    public AClass(Vector2 POS, GameServiceContainer SERVICES) : base("2D/Units/Heroes/raggedy.png", POS,
        new Vector2(25, 25) * GameState.Settings.ScreenRatio, 1, 1, 1000, SERVICES)
    {
        Services = SERVICES;
        // sets the values of the mob
        velo = 2.0f * GameState.Settings.ScreenRatio;
        range = 400.0f;
        attackTimer = new Timer(350);
    }

    public override void Update(Vector2 OFFSET, Hero HERO, Grid GRID)
    {
        base.Update(OFFSET, HERO, GRID);
    }


    public override void AI(Hero HERO, Grid GRID)
    {
        // Checks if the hero is in range or if the mob is attacking
        if (GetDist(HERO.pos, pos) < range - 50.0f || attacking)
        {
            // Sets the mob to attacking and updates the attack timer
            attacking = true;
            attackTimer.UpdateTimer();

            // Returns if the attack timer is not done
            if (!attackTimer.Test()) return;

            // adds a projectile to the game
            World.AddProjectile(new LaserSlash(new Vector2(pos.X, pos.Y), this,
                new Vector2(HERO.pos.X, HERO.pos.Y), Services));

            // resets the attack timer and sets the mob to not attacking
            attackTimer.ResetToZero();
            attacking = false;
        }
        else
        {
            // If the hero is moving or the path nodes are null, it finds a path
            if (PathNodes == null || HERO.IsMoving || lastHeroPos != HERO.pos)
            {
                // Finds the path
                PathNodes = FindPath(HERO, GRID);

                //Cache the last position of the hero
                lastHeroPos = HERO.pos;
            }

            // If the path nodes are not null, it moves
            Move();
        }
    }

    // Returns a list of path nodes using BFS from the mob to the hero
    public List<Vector2>? FindPath(Hero HERO, Grid GRID)
    {
        // Empties the path nodes
        PathNodes?.Clear();

        var path = GRID.AStarSearch(pos, new Vector2(HERO.pos.X, HERO.pos.Y));

        // Returns the position of the path nodes
        return path is null or { Count: <= 0 } ? null : path.Select(VARIABLE => VARIABLE.pos).ToList();
    }

    public void Move()
    {
        // If path nodes isn't null and has a count greater than 0, it removes the first node and sets the next node to the first node
        if (PathNodes is { Count: > 0 })
        {
            PathNodes.RemoveAt(0);

            if (PathNodes.Count > 0) NextNode = PathNodes[0];
        }

        // Moves towards the next node
        pos += RsmbMovement(NextNode, pos, velo);
        rot = RotateTowards(pos, NextNode);
    }

    public override void Draw(Vector2 OFFSET, GameServiceContainer SERVICES)
    {
        base.Draw(OFFSET, SERVICES);
    }
}