using System.Collections.Generic;
using Panda.MGModel.Projectiles;

namespace Panda.MGModel;

// A natsu hero
public class Natsu : Hero
{
    public Natsu(GameServiceContainer SERVICES) : base("2D/Units/Heroes/Natsu.png",
        new Vector2(GameState.ScreenWidth / 2, GameState.ScreenHeight / 2),
        new Vector2(25, 30) * GameState.Settings.ScreenRatio, 1, 8, 75, SERVICES)
    {
        velo = 6.0f * GameState.Settings.ScreenRatio;
    }

    public override void Update(Vector2 OFFSET, Grid GRID, List<Mob?> MOBS)
    {
        base.Update(OFFSET, GRID, MOBS);
    }

    public override void Invoke(List<Mob?> MOBS)
    {
        base.Invoke(MOBS);
    }

    // Creates a fireball projectile
    protected override BaseProjectile CreateProjectile(Vector2 TARGET)
    {
        return new Fireball(new Vector2(pos.X, pos.Y), this, TARGET, Services);
    }

    public override void Draw(Vector2 OFFSET, GameServiceContainer SERVICES)
    {
        base.Draw(OFFSET, SERVICES);
    }
}