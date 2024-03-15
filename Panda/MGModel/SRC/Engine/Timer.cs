using System;

namespace Panda.MGModel.SRC.Engine;

// The timer
public sealed class Timer
{
    private TimeSpan timer { get; set; }

    public static GameTime gameTime { get; set; }

    // The time in milliseconds
    private readonly int milliseconds;

    public Timer(int MSEC)
    {
        // sets the time in milliseconds
        milliseconds = MSEC;
    }

    // adds the elapsed time to the timer
    public void UpdateTimer()
    {
        timer += gameTime.ElapsedGameTime;
    }

    // returns true if the timer is greater than or equal to the time in milliseconds
    public bool Test()
    {
        return timer.TotalMilliseconds >= milliseconds;
    }

    // resets the timer to zero
    public void ResetToZero()
    {
        timer = TimeSpan.Zero;
    }
}