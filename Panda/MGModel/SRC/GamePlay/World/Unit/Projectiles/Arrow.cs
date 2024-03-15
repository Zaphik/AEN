namespace Panda.MGModel.Projectiles;

// An arrow projectile
public class Arrow : BaseProjectile
{
    public Arrow(Vector2 POS, AtkObject SENDER, Vector2 TARGET, GameServiceContainer SERVICES) : base(
        "2D/Units/Projectiles/Arrow.png", POS, new Vector2(25, 25) * GameState.Settings.ScreenRatio, SENDER, TARGET, 5,
        1, 50, SERVICES)
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