using SkiaSharp;
using DmdSkiaFlex.Sprites;

namespace DmdSkiaFlex.Core;

public sealed class DmdEngine
{
    public DmdCanvas Canvas { get; }
    public DmdOptions Options { get; } = new();

    private readonly DmdTextScroller _scroller = new();
    private readonly Stopwatch _sw = Stopwatch.StartNew();
    private long _lastMs;

    private IDmdSprite? _logoColor;
    private IDmdSprite? _logoMono;

    public DmdEngine(int width, int height)
    {
        Canvas = new DmdCanvas(width, height);

        _scroller.Text = "DMD SKIA FLEX  /api/text?text=HELLO  ";
        _scroller.Y = 2;
        _scroller.SpeedPxPerSec = 35;
        _scroller.Color = DmdColor.Amber;
        _scroller.Intensity = 255;
        _scroller.Reset(width);
    }

    public void TryLoadSampleSprites()
    {
        var dir = Path.Combine(AppContext.BaseDirectory, "assets");
        var path = Path.Combine(dir, "logo.png");
        if (!File.Exists(path))
            return;

        using var bmp = SKBitmap.Decode(path);
        if (bmp is null) return;

        _logoColor = new PngColorSprite(bmp.Copy());
        _logoMono = new PngMonoSprite(bmp.Copy(), DmdColor.Amber);
    }

    public void SetScrollingText(string text)
    {
        _scroller.Text = string.IsNullOrWhiteSpace(text) ? "" : text;
        _scroller.Reset(Canvas.Width);
    }

    public void Step()
    {
        var ms = _sw.ElapsedMilliseconds;
        var dt = (ms - _lastMs) / 1000f;
        _lastMs = ms;

        Canvas.Clear(Options.Background.A == 0 ? null : Options.Background);

        // Demo: time on top row
        var now = DateTime.Now;
        Canvas.DrawText($"TIME {now:HH:mm:ss}", 2, 0, Options.DefaultTextColor, 230);

        // Demo: scrolling text
        _scroller.Update(dt);
        _scroller.Render(Canvas);
        _scroller.WrapIfNeeded(Canvas);

        // Demo: sprite in top-right
        // Switch sprite type by seconds (color vs mono) to show flexibility.
        var useColor = (now.Second % 2) == 0;
        var sprite = useColor ? _logoColor : _logoMono;

        if (sprite != null)
        {
            int sx = Canvas.Width - sprite.Width - 2;
            int sy = Canvas.Height - sprite.Height - 2;
            Canvas.DrawSprite(sprite, sx, sy, useColor ? DmdBlendMode.Alpha : DmdBlendMode.Max, opacity: 1f, intensityScale: 255);
        }
    }
}

public sealed class DmdOptions
{
    public DmdColor Background { get; set; } = DmdColor.Transparent;
    public DmdColor DefaultTextColor { get; set; } = DmdColor.Amber;
    public DmdBlendMode DefaultBlend { get; set; } = DmdBlendMode.Max;
}
