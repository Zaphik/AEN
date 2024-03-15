using System;
using System.IO;


namespace Panda.MGModel;

// Ok yes this is pretty global
// But also, the stuff in here are all never gonna change
// They are actually static and make sense to be in a global class
public abstract class FileConsts
{
    //Takes us to the content directory
    public static readonly string BaseDir =
        Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory[
                ..AppDomain.CurrentDomain.BaseDirectory.IndexOf("bin", StringComparison.Ordinal)], "Content");

    //To save my suffering
    public static readonly string Motivation = Path.Combine(BaseDir, "Audio/Motivation");
    public static readonly string Error = Path.Combine(BaseDir, "Error.png");

    //Path to the savefile
    public static readonly string SettingsFile = Path.Combine(BaseDir, "Settings.json");
    public static readonly string SaveFile = Path.Combine(BaseDir, "SaveData.json");
}