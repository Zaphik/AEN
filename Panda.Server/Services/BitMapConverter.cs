using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Panda.Server.Services;

// This class is used to load images from a given path
// Credits to https://docs.avaloniaui.net/docs/guides/data-binding/how-to-bind-image-files
public static class BitMapConverter
{
    public static Bitmap? LoadFromResource(Uri? resourceUri)
    {
        if (resourceUri == null) return null;

        try
        {
            return new Bitmap(AssetLoader.Open(resourceUri));
        }
        catch
        {
            return null;
        }
    }
}