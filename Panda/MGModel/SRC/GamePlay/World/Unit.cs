namespace Panda.MGModel;

// The base class for all units
public class Unit : AtkObject
{
    protected Unit(string PATH, Vector2 POS, Vector2 SIZE, int ROWS, int COLUMNS, int TICK,
        GameServiceContainer SERVICES) : base(PATH, POS, SIZE, ROWS, COLUMNS, TICK, SERVICES)
    {
    }

    public override void Update(Vector2 OFFSET)
    {
        base.Update(OFFSET);
    }

    public override void Draw(Vector2 OFFSET, GameServiceContainer SERVICES)
    {
        base.Draw(OFFSET, SERVICES);
    }
}