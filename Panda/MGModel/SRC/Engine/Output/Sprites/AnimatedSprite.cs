using Panda.MGModel.SRC.Engine;

namespace Panda.MGModel;

public class AnimatedSprite : BaseSprite
{
    // The current frame and row
    private int currFrame;
    private int currRow;

    // The number of rows and columns
    private readonly int rows;
    private readonly int columns;

    // The frame timer
    private readonly Timer frameTimer;

    protected AnimatedSprite(string PATH, Vector2 POS, Vector2 SIZE, int ROWS, int COLUMNS, int TICK,
        GameServiceContainer SERVICES)
        : base(PATH, POS, SIZE, SERVICES)
    {
        // sets the number of rows and columns
        rows = ROWS;
        columns = COLUMNS;

        // sets the current frame and row to 0
        currFrame = 0;
        currRow = 0;

        // creates the frame timer
        frameTimer = new Timer(TICK);
    }

    public override void Update()
    {
        // updates the frame timer
        frameTimer.UpdateTimer();

        // if the frame timer has expired
        if (frameTimer.Test())
        {
            // if there are more than one column
            if (columns > 1)
            {
                // increments the current frame
                currFrame = (currFrame + 1) % columns;
                if (currFrame == 0 && rows > 1) currRow = (currRow + 1) % rows;
            }
            // if there are more than one row
            else if (rows > 1)
            {
                // increments the current row
                currRow = (currRow + 1) % rows;
            }

            // resets the frame timer
            frameTimer.ResetToZero();
        }

        base.Update();
    }

    public override void Draw(Vector2 OFFSET, GameServiceContainer SERVICES)
    {
        // draws the sprite with the current frame and row
        SERVICES.GetService<SpriteBatch>().Draw(_tx,
            new Rectangle((int)(pos.X + OFFSET.X), (int)(pos.Y + OFFSET.Y), (int)size.X, (int)size.Y),
            new Rectangle(_tx.Bounds.Width / columns * currFrame, _tx.Bounds.Height / rows * currRow,
                _tx.Bounds.Width / columns, _tx.Bounds.Height / rows), Color.White, rot,
            new Vector2(_tx.Bounds.Width / columns / 2, _tx.Bounds.Height / rows / 2), SpriteFX, 0);
    }
}