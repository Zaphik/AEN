namespace Panda.MGModel;

// A BClass mob
public class BClass : Mob
{
    protected internal BClass(Vector2 POS, GameServiceContainer SERVICES) : base("2D/Units/Mobs/BClass.png", POS,
        new Vector2(25, 25) * GameState.Settings.ScreenRatio, 1, 1, 1000, SERVICES)
    {
        velo = 1.5f * GameState.Settings.ScreenRatio;
    }

    public override void Update(Vector2 OFFSET, Hero HERO, Grid GRID)
    {
        base.Update(OFFSET, HERO, GRID);
    }

    public override void AI(Hero HERO, Grid GRID)
    {
        base.AI(HERO, GRID);
    }

    public override void Draw(Vector2 OFFSET, GameServiceContainer SERVICES)
    {
        base.Draw(OFFSET, SERVICES);
    }
}