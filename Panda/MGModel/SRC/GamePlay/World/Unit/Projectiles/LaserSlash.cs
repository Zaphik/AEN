namespace Panda.MGModel.Projectiles;

// A laser slash projectile
public class LaserSlash : BaseProjectile
{
    public LaserSlash(Vector2 POS, AtkObject SENDER, Vector2 TARGET, GameServiceContainer SERVICES) : base(
        "2D/Units/Projectiles/LaserSlash.png", POS, new Vector2(25, 25) * GameState.Settings.ScreenRatio, SENDER,
        TARGET, 1, 4, 50, SERVICES)
    {
        velo = 5.30f * GameState.Settings.ScreenRatio;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Draw(Vector2 OFFSET, GameServiceContainer SERVICES)
    {
        base.Draw(OFFSET, SERVICES);
    }
}