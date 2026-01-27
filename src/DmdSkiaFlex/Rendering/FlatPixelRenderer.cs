using DmdSkiaFlex.Core;
using SkiaSharp;

namespace DmdSkiaFlex.Rendering;

public sealed class FlatPixelRenderer : IDmdOutputRenderer
{
    public byte[] RenderPng(DmdCanvas canvas, int scale)
    {
        int w = canvas.Width * scale;
        int h = canvas.Height * scale;

        using var surface = SKSurface.Create(new SKImageInfo(w, h, SKColorType.Rgba8888, SKAlphaType.Premul));
        var c = surface.Canvas;
        c.Clear(SKColors.Transparent);

        using var paint = new SKPaint
        {
            IsAntialias = false,
            Style = SKPaintStyle.Fill
        };

        for (int y = 0; y < canvas.Height; y++)
        {
            for (int x = 0; x < canvas.Width; x++)
            {
                var px = canvas.GetPixel(x, y);
                if (px.Intensity == 0 || px.Color.A == 0) continue;

                var col = ApplyIntensity(px);
                paint.Color = col;
                c.DrawRect(x * scale, y * scale, scale, scale, paint);
            }
        }

        using var img = surface.Snapshot();
        using var data = img.Encode(SKEncodedImageFormat.Png, 100);
        return data.ToArray();
    }

    private static SKColor ApplyIntensity(DmdPixel px)
    {
        // multiply RGB by intensity (DMD-style)
        float t = px.Intensity / 255f;
        byte L(byte v) => (byte)Math.Clamp((int)(v * t), 0, 255);
        return new SKColor(L(px.Color.R), L(px.Color.G), L(px.Color.B), px.Color.A);
    }
}
