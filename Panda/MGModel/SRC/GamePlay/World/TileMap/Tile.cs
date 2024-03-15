namespace Panda.MGModel;

// reason for this class is to get rid of any outline missing from integer rounding
public class Tile : BaseSprite
{
    public Tile(string PATH, Vector2 POS, Vector2 SIZE, GameServiceContainer SERVICES)
        : base(PATH, POS, SIZE, SERVICES)
    {
        size = new Vector2(SIZE.X + 1, SIZE.Y + 1);
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