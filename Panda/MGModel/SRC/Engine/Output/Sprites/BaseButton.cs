namespace Panda.MGModel;

//Credit to https://www.youtube.com/@GameDevQuickie for the using rectangle intersection to check if the cursor is hovering over the button
public class BaseButton : BaseSprite
{
    // The rectangle of the button
    private readonly Rectangle rect;

    private GameServiceContainer Services { get; set; }

    public BaseButton(string PATH, Vector2 POS, Vector2 SIZE, GameServiceContainer SERVICES) : base(PATH, POS, SIZE,
        SERVICES)
    {
        // sets the rectangle of the button
        rect = new Rectangle((int)(pos.X - size.X / 2), (int)(pos.Y - size.Y / 2), (int)size.X, (int)size.Y);

        // sets the colour of the button to white
        Colour = Color.White;

        Services = SERVICES;
    }


    // Returns true if the button is clicked
    private bool IsClicked => Services.GetService<Mouse>().Cursor.Intersects(rect) &&
                              Services.GetService<Mouse>().LeftSingleClick();

    // Returns true if the cursor is hovering over the button
    private bool IsHovered => Services.GetService<Mouse>().Cursor.Intersects(rect);

    public new virtual void Update()
    {
        // sets the colour of the button to pink if it is clicked
        if (IsClicked)
        {
            Colour = Color.Pink;
            Invoke();
        }
        else
        {
            // sets the colour of the button to brown if the cursor is hovering over it
            Colour = IsHovered ? Color.Brown : Color.White;
        }
    }

    // Method that does something when the button is clicked
    protected virtual void Invoke()
    {
    }

    // Draws the button
    public virtual void Draw(GameServiceContainer SERVICES)
    {
        SERVICES.GetService<SpriteBatch>().Draw(_tx, pos, new Rectangle(0, 0, _tx.Bounds.Width, _tx.Bounds.Height),
            Colour, 0f,
            new Vector2(_tx.Bounds.Width / 2, _tx.Bounds.Height / 2),
            (Vector2)new Vector2(rect.Width / (float)_tx.Width, rect.Height / (float)_tx.Height), SpriteEffects.None,
            0f);
    }
}