using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;


namespace Panda.MGModel;

// Credits to http://rbwhitaker.wikidot.com/monogame-keyboard-input for the idea of caching the previous keyboard state
public sealed class Keyboard
{
    // The two keyboard states
    private KeyboardState currKeyboard { get; set; }
    private KeyboardState prevKeyboard { get; set; }

    // The list of pressed keys
    private List<string> pressedKeys { get; set; }
    private Keys key { get; set; }

    public void Update()
    {
        // caches the previous keyboard state
        prevKeyboard = currKeyboard;

        // gets the current keyboard state
        currKeyboard = Microsoft.Xna.Framework.Input.Keyboard.GetState();

        pressedKeys = [];

        // gets the list of pressed keys
        pressedKeys.Clear();
        for (var i = currKeyboard.GetPressedKeys().Length - 1; i >= 0; i--)
            pressedKeys.Add(currKeyboard.GetPressedKeys()[i].ToString());
    }

    public bool GetSinglePress(string KEY)
    {
        // returns true if the key is pressed and was not pressed in the previous frame
        key = (Keys)Enum.Parse(typeof(Keys), KEY);
        return currKeyboard.IsKeyDown(key) && prevKeyboard.IsKeyUp(key);
    }

    public bool GetPress(string KEY)
    {
        // returns true if the key is pressed
        return pressedKeys.Contains(KEY);
    }
}