namespace Panda.MGModel;

public class GridLoc
{
    // The state of the grid location
    public bool IsFilled { get; private set; }

    // Fills the grid location
    public void Fill(bool value)
    {
        IsFilled = value;
    }

    // The position of the grid location
    public Vector2 pos { get; set; }


    public GridLoc(bool FILLED)
    {
        IsFilled = FILLED;
        Fill(FILLED);
    }
}