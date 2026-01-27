using System.Runtime.CompilerServices;

namespace DmdSkiaFlex.Core;

public sealed class DmdCanvas : IDmdCanvas
{
    private readonly DmdPixel[,] _buf;

    public int Width { get; }
    public int Height { get; }

    public DmdCanvas(int width, int height)
    {
        Width = width;
        Height = height;
        _buf = new DmdPixel[height, width];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DmdPixel GetPixel(int x, int y) => _buf[y, x];

    public void Clear(DmdColor? color = null)
    {
        var px = color is null
            ? DmdPixel.Off
            : new DmdPixel(255, color.Value);

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                _buf[y, x] = px;
            }
        }
    }

    public void SetPixel(int x, int y, DmdPixel src, DmdBlendMode mode = DmdBlendMode.Max)
    {
        if ((uint)x >= (uint)Width || (uint)y >= (uint)Height) return;
        if (src.Intensity == 0 || src.Color.A == 0) return;

        var dst = _buf[y, x];
        _buf[y, x] = mode switch
        {
            DmdBlendMode.Max => MaxBlend(dst, src),
            DmdBlendMode.Alpha => AlphaBlend(dst, src),
            DmdBlendMode.Additive => AddBlend(dst, src),
            _ => MaxBlend(dst, src)
        };
    }

    public void DrawSprite(IDmdSprite sprite, int x, int y, DmdBlendMode mode = DmdBlendMode.Max, float opacity = 1f, byte intensityScale = 255)
    {
        int dstW = Width;
        int dstH = Height;

        int startX = Math.Max(0, -x);
        int startY = Math.Max(0, -y);
        int endX = Math.Min(sprite.Width, dstW - x);
        int endY = Math.Min(sprite.Height, dstH - y);

        if (startX >= endX || startY >= endY) return;

        for (int sy = startY; sy < endY; sy++)
        {
            int yy = y + sy;
            for (int sx = startX; sx < endX; sx++)
            {
                int xx = x + sx;
                var p = sprite.GetPixel(sx, sy);
                if (p.Intensity == 0 || p.Color.A == 0) continue;

                int i = p.Intensity * intensityScale / 255;
                i = (int)(i * opacity);
                if (i <= 0) continue;

                var scaled = new DmdPixel((byte)Math.Min(255, i), p.Color);
                SetPixel(xx, yy, scaled, mode);
            }
        }
    }

    public void DrawText(string text, int x, int y, DmdColor color, byte intensity = 255)
        => Font5x7.DrawText(this, text, x, y, color, intensity);

    public int MeasureText(string text) => Font5x7.MeasureText(text);

    private static DmdPixel MaxBlend(DmdPixel dst, DmdPixel src)
        => (src.Intensity >= dst.Intensity) ? src : dst;

    private static DmdPixel AddBlend(DmdPixel dst, DmdPixel src)
    {
        int i = Math.Min(255, dst.Intensity + src.Intensity);
        var c = (src.Intensity >= dst.Intensity) ? src.Color : dst.Color;
        return new DmdPixel((byte)i, c);
    }

    private static DmdPixel AlphaBlend(DmdPixel dst, DmdPixel src)
    {
        float a = (src.Color.A / 255f) * (src.Intensity / 255f);
        float inv = 1f - a;

        byte L(byte d, byte s) => (byte)Math.Clamp((int)(d * inv + s * a), 0, 255);

        var outColor = new DmdColor(
            L(dst.Color.R, src.Color.R),
            L(dst.Color.G, src.Color.G),
            L(dst.Color.B, src.Color.B),
            255);

        int outI = Math.Clamp((int)(dst.Intensity * inv + src.Intensity * a), 0, 255);
        return new DmdPixel((byte)outI, outColor);
    }
}
