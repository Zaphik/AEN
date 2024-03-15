namespace Panda.Server;

// Settings model that replicate the database table
public class Settings
{
    public int? SettingsID { get; init; }
    public string Name { get; set; }
    public string Up { get; set; }
    public string Down { get; set; }
    public string Left { get; set; }
    public string Right { get; set; }
    public string Reset { get; set; }


    public float Volume { get; set; }
    public float ScreenRatio { get; set; }

    public string Build { get; set; }
    public int? UserID { get; init; }
}