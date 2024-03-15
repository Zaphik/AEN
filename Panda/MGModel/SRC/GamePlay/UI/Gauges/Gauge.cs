using Panda.MGModel;

public class Gauge : BaseSprite
{
    // The source rectangle for the gauge
    private Rectangle sourceRect;

    // The current and maximum size of the gauge
    protected float currSize { get; set; }
    protected float maxSize { get; set; }

    // The background of the gauge
    private readonly BaseSprite background;

    protected Gauge(string PATH, Vector2 POS, Vector2 SIZE, GameServiceContainer SERVICES) : base(PATH, POS, SIZE,
        SERVICES)
    {
        // Sets the background of the gauge
        background = new BaseSprite("2D/Misc/Grid.png", POS, SIZE + new Vector2(4), SERVICES);

        // Sets the source rectangle to the full size of the gauge
        sourceRect = new Rectangle(0, 0, _tx.Width, _tx.Height);

        // Make the initial colour green
        Colour = Color.Green;
    }

    public new virtual void Update()
    {
        // Updates the source rectangle and the colour of the gauge
        sourceRect.Width = (int)(_tx.Width * currSize / maxSize);

        // Changes the colour of the gauge based on the current size
        Colour = (currSize / maxSize) switch
        {
            > 0.75f => Color.Green,
            > 0.5f => Color.Yellow,
            > 0.25f => Color.Orange,
            _ => Color.Red
        };
    }


    public virtual void Draw(GameServiceContainer SERVICES)
    {
        // Draws the background of the gauge
        SERVICES.GetService<SpriteBatch>().Draw(background._tx,
            new Rectangle((int)pos.X - 1, (int)pos.Y - 1, (int)size.X + 2, (int)size.Y + 2), Color.White);

        // Draws the gauge
        SERVICES.GetService<SpriteBatch>().Draw(_tx,
            new Rectangle((int)pos.X, (int)pos.Y, (int)size.X * sourceRect.Width / _tx.Width, (int)size.Y), sourceRect,
            Colour);
    }
}