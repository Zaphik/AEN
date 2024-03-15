using System;
using Panda.MGModel.SRC.Engine;

namespace Panda.MGModel;

// The main game

public class Pandemonium : Game
{
    // Gameplay and audio
    private GamePlay gameplay { get; set; }
    private BaseAudio Audio { get; set; }

    // Monogame rendering shenanigans
    private SpriteBatch _spriteBatch { get; set; }
    private ContentManager _content { get; set; }
    private GraphicsDeviceManager _graphics { get; set; }

    // Input devices
    private Keyboard _keyboard { get; set; }
    private Mouse _mouse { get; set; }


    //The holy grail
    private GameServiceContainer services { get; set; }

    public static Action ExitGame { get; private set; }

    public Pandemonium()
    {
        _graphics = new GraphicsDeviceManager(this);
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        IsMouseVisible = true;
        Window.IsBorderless = true;
        GameState.ScreenHeight = (int)(500 * GameState.Settings.ScreenRatio);
        GameState.ScreenWidth = (int)(800 * GameState.Settings.ScreenRatio);


        _graphics.PreferredBackBufferWidth = GameState.ScreenWidth;
        _graphics.PreferredBackBufferHeight = GameState.ScreenHeight;

        _graphics.ApplyChanges();

        ExitGame = Exit;


        base.Initialize();
    }

    protected override void LoadContent()
    {
        _content = Content;
        _content.RootDirectory = "Content";
        _spriteBatch = new SpriteBatch(_graphics.GraphicsDevice);


        // TODO: use this.Content to load your game content here


        _keyboard = new Keyboard();
        _mouse = new Mouse();

        services = new GameServiceContainer();
        services.AddService(typeof(SpriteBatch), _spriteBatch);
        services.AddService(typeof(ContentManager), _content);
        services.AddService(typeof(GraphicsDevice), _graphics.GraphicsDevice);
        services.AddService(typeof(Keyboard), _keyboard);
        services.AddService(typeof(Mouse), _mouse);


        Audio = new BaseAudio("Audio/yosei.wav", GameState.Settings.Volume, true, services);
        gameplay = new GamePlay(services);
    }

    protected override void Update(GameTime gameTime)
    {
        // TODO: Add your update logic here


        Timer.gameTime = gameTime;

        _keyboard.Update();
        _mouse.Update();

        gameplay.Update();

        base.Update(Timer.gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        _graphics.GraphicsDevice.Clear(Color.Orchid);

        // TODO: Add your drawing code here

        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
        gameplay.Draw(services);

        _spriteBatch.End();

        base.Draw(Timer.gameTime);
    }
}