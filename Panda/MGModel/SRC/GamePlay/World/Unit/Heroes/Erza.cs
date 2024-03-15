using System.Collections.Generic;
using Panda.MGModel.Projectiles;

namespace Panda.MGModel;

// An erza hero
public class Erza : Hero
{
    public Erza(GameServiceContainer SERVICES) : base("2D/Units/Heroes/Erza.png",
        new Vector2(GameState.ScreenWidth / 2, GameState.ScreenHeight / 2),
        new Vector2(25, 25) * GameState.Settings.ScreenRatio, 2, 5, 80, SERVICES)
    {
        velo = 4.0f * GameState.Settings.ScreenRatio;
    }

    public override void Update(Vector2 OFFSET, Grid GRID, List<Mob?> MOBS)
    {
        base.Update(OFFSET, GRID, MOBS);
    }

    public override void Invoke(List<Mob?> MOBS)
    {
        base.Invoke(MOBS);
    }

    // Creates a firestar projectile
    protected override BaseProjectile CreateProjectile(Vector2 TARGET)
    {
        return new FireStar(new Vector2(pos.X, pos.Y), this, TARGET, Services);
    }

    public override void Draw(Vector2 OFFSET, GameServiceContainer SERVICES)
    {
        base.Draw(OFFSET, SERVICES);
    }
}