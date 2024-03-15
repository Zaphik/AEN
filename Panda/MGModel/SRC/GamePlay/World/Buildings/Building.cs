using System.Collections.Generic;

namespace Panda.MGModel;

public class Building : SpawnPoint
{
    protected Building(string PATH, Vector2 POS, Vector2 SIZE, int ROWS, int COLUMNS, int TICK,
        GameServiceContainer SERVICES) : base(PATH, POS, SIZE,
        ROWS, COLUMNS, TICK, SERVICES)
    {
    }


    // takes in a list of mobs as well
    public virtual void Update(Vector2 OFFSET, List<Mob?> MOBS)
    {
        base.Update(OFFSET);
    }

    protected override void Invoke()
    {
        base.Invoke();
    }


    public override void Draw(Vector2 OFFSET, GameServiceContainer SERVICES)
    {
        base.Draw(OFFSET, SERVICES);
    }
}