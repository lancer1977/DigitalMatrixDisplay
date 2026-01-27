using DmdSkiaFlex.Core;
using SkiaSharp;

namespace DmdSkiaFlex.Sprites;

public sealed class PngMonoSprite : IDmdSprite
{
    private readonly SKBitmap _bmp;
    private readonly DmdColor _tint;

    public int Width => _bmp.Width;
    public int Height => _bmp.Height;

    public PngMonoSprite(SKBitmap bmp, DmdColor tint)
    {
        _bmp = bmp;
        _tint = tint;
    }

    public DmdPixel GetPixel(int x, int y)
    {
        var c = _bmp.GetPixel(x, y);
        if (c.Alpha == 0) return DmdPixel.Off;

        float luma = (0.2126f * c.Red + 0.7152f * c.Green + 0.0722f * c.Blue) / 255f;
        float a = c.Alpha / 255f;
        byte intensity = (byte)Math.Clamp((int)(luma * a * 255f), 0, 255);

        return new DmdPixel(intensity, new DmdColor(_tint.R, _tint.G, _tint.B, 255));
    }
}
