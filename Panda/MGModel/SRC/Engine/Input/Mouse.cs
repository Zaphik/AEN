using Microsoft.Xna.Framework.Input;

namespace Panda.MGModel;

// Credits to http://rbwhitaker.wikidot.com/monogame-mouse-input for the idea of caching the previous mouse state
public sealed class Mouse
{
    // The two mouse states
    private MouseState CurrMouse { get; set; }
    private MouseState PrevMouse { get; set; }

    public void Update()
    {
        // caches the previous mouse state
        PrevMouse = CurrMouse;

        // gets the current mouse state
        CurrMouse = Microsoft.Xna.Framework.Input.Mouse.GetState();
    }

    public bool LeftSingleClick()
    {
        // returns true if the left mouse button is pressed and was not pressed in the previous frame
        return CurrMouse.LeftButton == ButtonState.Pressed && PrevMouse.LeftButton == ButtonState.Released;
    }

    // The rectangle of the cursor
    public Rectangle Cursor => new(CurrMouse.Position.X, CurrMouse.Position.Y, 1, 1);
}