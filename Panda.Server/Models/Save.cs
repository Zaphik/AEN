using Avalonia.Media.Imaging;
using Panda.Server.Services;

namespace Panda.Server;

//Save model that replicate the database table but also converts the choice to an image
public class Save
{
    public Save()
    {
    }

    public Save(string? choice)
    {
        Choice = choice;
        Image = BitMapConverter.LoadFromResource(new Uri($"avares://Panda/Resources/Images/{choice}.png"));
    }

    public int? SaveID { get; init; }
    public string? Choice { get; set; }
    public DateTime LastPlayed { get; set; }
    public int? UserID { get; init; }
    public Bitmap? Image { get; set; }
}