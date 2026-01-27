using DmdSkiaFlex.Core;
using SkiaSharp;

namespace DmdSkiaFlex.Rendering;

public sealed class PinballDotsRenderer : IDmdOutputRenderer
{
    public byte[] RenderPng(DmdCanvas canvas, int scale)
    {
        int outW = canvas.Width * scale;
        int outH = canvas.Height * scale;

        using var surface = SKSurface.Create(new SKImageInfo(outW, outH, SKColorType.Rgba8888, SKAlphaType.Premul));
        var sk = surface.Canvas;

        sk.Clear(SKColors.Transparent);

        float cell = scale;
        float dotInset = cell * 0.08f;
        float dotR = (cell * 0.45f) - dotInset;

        // Optional: faint unlit dot grid color
        var unlit = new SKColor(25, 10, 0, 160);

        using var paint = new SKPaint
        {
            IsAntialias = true,
            Style = SKPaintStyle.Fill
        };

        // Draw dots
        for (int y = 0; y < canvas.Height; y++)
        {
            for (int x = 0; x < canvas.Width; x++)
            {
                var px = canvas.GetPixel(x, y);
                float cx = x * cell + cell / 2f;
                float cy = y * cell + cell / 2f;

                if (px.Intensity == 0 || px.Color.A == 0)
                {
                    paint.Color = unlit;
                    sk.DrawCircle(cx, cy, dotR, paint);
                    continue;
                }

                paint.Color = ApplyIntensity(px);
                sk.DrawCircle(cx, cy, dotR, paint);
            }
        }

        // Scanlines + subtle vignette
        DrawScanlines(sk, outW, outH);
        DrawVignette(sk, outW, outH);

        using var img = surface.Snapshot();
        using var data = img.Encode(SKEncodedImageFormat.Png, 100);
        return data.ToArray();
    }

    private static SKColor ApplyIntensity(DmdPixel px)
    {
        float t = px.Intensity / 255f;
        byte L(byte v) => (byte)Math.Clamp((int)(v * t), 0, 255);
        return new SKColor(L(px.Color.R), L(px.Color.G), L(px.Color.B), px.Color.A);
    }

    private static void DrawScanlines(SKCanvas canvas, int w, int h)
    {
        using var p = new SKPaint
        {
            IsAntialias = false,
            Style = SKPaintStyle.Fill,
            Color = new SKColor(0, 0, 0, 22)
        };

        for (int y = 0; y < h; y += 2)
            canvas.DrawRect(0, y, w, 1, p);
    }

    private static void DrawVignette(SKCanvas canvas, int w, int h)
    {
        // Simple radial darkening to make it feel "glassy"
        using var paint = new SKPaint { IsAntialias = true };
        var center = new SKPoint(w / 2f, h / 2f);
        float radius = MathF.Max(w, h) * 0.75f;

        paint.Shader = SKShader.CreateRadialGradient(
            center,
            radius,
            new[] { new SKColor(0, 0, 0, 0), new SKColor(0, 0, 0, 70) },
            new[] { 0f, 1f },
            SKShaderTileMode.Clamp);

        canvas.DrawRect(0, 0, w, h, paint);
    }
}
