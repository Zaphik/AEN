using System;
using System.IO;
using Microsoft.Xna.Framework.Audio;

namespace Panda.MGModel;

public class BaseAudio
{
    // The sound effect
    private SoundEffect sfx { get; set; }

    public BaseAudio(string PATH, float VOL, bool LOOP, GameServiceContainer SERVICES)
    {
        // gets the path into the content directory
        PATH = Path.Combine(FileConsts.BaseDir, PATH);
        try
        {
            using (var fileStream = new FileStream(PATH, FileMode.Open))
            {
                // loads the sound effect from the file
                sfx = SoundEffect.FromStream(fileStream);
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Error loading original content: " + PATH);
            try
            {
                //If the file is not found, it tries to load the file without the extension in case it was an xnb
                PATH = Path.ChangeExtension(PATH, null);
                sfx = SERVICES.GetService<ContentManager>().Load<SoundEffect>(PATH);
            }
            catch (Exception)
            {
                Console.WriteLine("Error loading content: " + PATH);

                //If the file is not found, it loads the error sound effect
                sfx = SERVICES.GetService<ContentManager>().Load<SoundEffect>(FileConsts.Motivation);
            }
        }

        // sets the volume and loops the sound effect if needed
        var sfxInst = sfx.CreateInstance();
        sfxInst.Volume = VOL;
        sfxInst.IsLooped = LOOP;
        sfxInst.Play();
    }
}