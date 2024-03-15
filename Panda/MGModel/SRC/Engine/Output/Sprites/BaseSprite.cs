using System;
using System.IO;


namespace Panda.MGModel;

// Credit to http://rbwhitaker.wikidot.com/monogame-spritebatch-basics for how to load and draw the texture
// Credit to https://flibitijibibo.com/xnacontent.html for how to load the texture from a file instead of using some dead file format because of "Better cross-platform support" even though it basically sucks trying to use the pipeline on anything but windows and even when you get it working using the mgcb editor cli which, btw, isn't even documented to the point of understanding for a beginner and I literally found out about it like yesterday
// And even then, it barely works on mac, i had to go on the dark web to find some version of the app from 1980 that actually worked because they wanted to make it a dotnet tool to make it 'integrated'
// IDE, it's legit in the name, 'Integrated' why would someone have to go out of their way to process their files into some universal file type, which if you read link #2, can at points degrade the quality of the asset
// So anyhow yh, this is the base class for all sprites
// Also, if you can't tell, I do love DI
// At one point, I did pass in content and graphics separately, but making the smallest of changes to the constructor of the base class would mean I would have to change the constructor of every single class that inherits from it
// Which is at least 28 classes
// So I just passed in the service container
// I did use globals at one point, but then you know why I changed it
public class BaseSprite
{
    // For rotation
    protected float rot { get; set; }

    // For position and size
    public Vector2 pos { get; set; }
    protected Vector2 size { get; set; }

    // For the texture
    public readonly Texture2D _tx;
    protected SpriteEffects SpriteFX { get; set; }
    protected Color Colour { get; set; }

    public BaseSprite(string PATH, Vector2 POS, Vector2 SIZE, GameServiceContainer SERVICES)
    {
        pos = POS;
        size = SIZE;

        SpriteFX = SpriteEffects.None;
        Colour = Color.White;

        //Gets the path into the Content Directory
        PATH = Path.Combine(FileConsts.BaseDir, PATH);


        try
        {
            //Loads the texture from the file
            using (var fileStream = new FileStream(PATH, FileMode.Open))
            {
                _tx = Texture2D.FromStream(SERVICES.GetService<GraphicsDevice>(), fileStream);
            }
        }
        catch (Exception)
        {
            try
            {
                //If the file is not found, it tries to load the file without the extension in case it was an xnb
                Console.WriteLine("Error loading original content: " + PATH);
                PATH = Path.ChangeExtension(PATH, null);
                _tx = SERVICES.GetService<ContentManager>().Load<Texture2D>(PATH);
            }
            catch (Exception)
            {
                using (var fileStream = new FileStream(FileConsts.Error, FileMode.Open))
                {
                    //If the file is not found, it loads the error texture
                    _tx = Texture2D.FromStream(SERVICES.GetService<GraphicsDevice>(), fileStream);
                }
            }
        }
    }

    public virtual void Update()
    {
    }


    //Let's mobs use smooth movement instead of jittering or teleportation
    public Vector2 RsmbMovement(Vector2 TARGET, Vector2 POS, float VELO)
    {
        var moveDir = TARGET - POS;
        moveDir.Normalize();
        return moveDir * VELO;
    }


    // Gets the distance between two points
    public float GetDist(Vector2 TARGET, Vector2 POS)
    {
        return Math.Abs(TARGET.X - POS.X) + Math.Abs(TARGET.Y - POS.Y);
    }

    // Rotates the sprite towards the target

    public float RotateTowards(Vector2 POS, Vector2 TARGET)
    {
        var dir = Vector2.Normalize(TARGET - POS);
        var angle = (float)Math.Acos(Vector2.Dot(dir, Vector2.UnitY));

        if (dir.X < 0) return (float)Math.PI + angle;

        return (float)Math.PI - angle;
    }

    //Draws the sprite
    public virtual void Draw(Vector2 OFFSET, GameServiceContainer SERVICES)
    {
        SERVICES.GetService<SpriteBatch>().Draw(_tx,
            new Rectangle((int)(pos.X + OFFSET.X), (int)(pos.Y + OFFSET.Y), (int)size.X, (int)size.Y),
            new Rectangle(0, 0, _tx.Bounds.Width, _tx.Bounds.Height),
            Colour, rot, new Vector2(_tx.Bounds.Width / 2, _tx.Bounds.Height / 2), SpriteFX, 0);
    }
}