using DmdSkiaFlex.Core;
using SkiaSharp;

namespace DmdSkiaFlex.Sprites;

public sealed class PngColorSprite : IDmdSprite
{
    private readonly SKBitmap _bmp;

    public int Width => _bmp.Width;
    public int Height => _bmp.Height;

    public PngColorSprite(SKBitmap bmp) => _bmp = bmp;

    public DmdPixel GetPixel(int x, int y)
    {
        var c = _bmp.GetPixel(x, y);
        if (c.Alpha == 0) return DmdPixel.Off;

        float luma = (0.2126f * c.Red + 0.7152f * c.Green + 0.0722f * c.Blue) / 255f;
        byte intensity = (byte)Math.Clamp((int)(luma * 255f), 0, 255);

        return new DmdPixel(intensity, new DmdColor(c.Red, c.Green, c.Blue, c.Alpha));
    }
}
