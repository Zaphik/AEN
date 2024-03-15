namespace Panda.MGModel;

// A DClass mob
public class DClass : Mob
{
    public DClass(Vector2 POS, GameServiceContainer SERVICES) : base("2D/Units/Mobs/DClass.png", POS,
        new Vector2(25, 25) * GameState.Settings.ScreenRatio, 1, 1, 1000, SERVICES)
    {
        velo = 2.0f * GameState.Settings.ScreenRatio;
    }

    public override void Update(Vector2 OFFSET, Hero HERO, Grid GRID)
    {
        base.Update(OFFSET, HERO, GRID);
    }

    public override void AI(Hero HERO, Grid GRID)
    {
        // Checks if the next node is filled and stops the mob from moving
        var nextNode = GRID.GetLocFromPos(pos + RsmbMovement(HERO.pos, pos, velo));

        if (nextNode is { IsFilled: true }) return;

        base.AI(HERO, GRID);
    }

    public override void Draw(Vector2 OFFSET, GameServiceContainer SERVICES)
    {
        base.Draw(OFFSET, SERVICES);
    }
}