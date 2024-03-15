namespace Panda.MGModel.Projectiles;

// A fireball projectile
public class Fireball : BaseProjectile
{
    public Fireball(Vector2 POS, AtkObject SENDER, Vector2 TARGET, GameServiceContainer SERVICES) : base(
        "2D/Units/Projectiles/boll.png", POS, new Vector2(48, 48) * GameState.Settings.ScreenRatio, SENDER, TARGET, 3,
        3, 100, SERVICES)
    {
        velo = 5.0f * GameState.Settings.ScreenRatio;
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