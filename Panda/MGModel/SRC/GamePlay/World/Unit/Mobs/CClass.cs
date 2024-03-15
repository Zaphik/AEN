using System;
using System.Collections.Generic;
using System.Linq;
using Panda.MGModel.SpawnPoints;

namespace Panda.MGModel;

public class CClass : Mob
{
    public static Action<SpawnPoint> AddSpawnPoint { get; set; }

    // The path nodes of the mob
    private List<Vector2>? PathNodes { get; set; }

    // The next node of the mob
    private Vector2 NextNode { get; set; }

    // The last position of the hero
    private Vector2 lastHeroPos { get; set; }

    private protected GameServiceContainer Services { get; set; }

    public CClass(Vector2 POS, GameServiceContainer SERVICES) : base("2D/Units/Mobs/CClass.png", POS,
        new Vector2(25, 25), 1, 1, 1000, SERVICES)
    {
        Services = SERVICES;
        // Sets the next node to the current position to trigger the pathfinding
        NextNode = new Vector2(POS.X, POS.Y);

        // Sets the values of the mob
        velo = 5.0f;
        currHealth = 3;
        maxHealth = currHealth;
    }

    public override void Update(Vector2 OFFSET, Hero HERO, Grid GRID)
    {
        if (Dead)
            // If dead, it spawns a DeadCclass
            AddSpawnPoint?.Invoke(new DeadCClass(new Vector2(pos.X, pos.Y), Services));

        base.Update(OFFSET, HERO, GRID);
    }


    public override void AI(Hero HERO, Grid GRID)
    {
        // Checks if the hero is in range and uses the base AI if it is
        if (GetDist(pos, HERO.pos) < 20)
        {
            base.AI(HERO, GRID);
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

        var path = GRID.BreadthFirstSearch(pos, new Vector2(HERO.pos.X, HERO.pos.Y));

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

    public override void GetHit(float DAMAGE)
    {
        base.GetHit(DAMAGE);
    }

    public override void Draw(Vector2 OFFSET, GameServiceContainer SERVICES)
    {
        base.Draw(OFFSET, SERVICES);
    }
}