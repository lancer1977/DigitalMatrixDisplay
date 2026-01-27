using DmdSkiaFlex.Core;
using DmdSkiaFlex.Rendering;

var builder = WebApplication.CreateBuilder(args);

// Bind to loopback for safety
builder.WebHost.UseUrls("http://127.0.0.1:5099");

builder.Services.AddSingleton<DmdEngine>(_ =>
{
    var engine = new DmdEngine(width: 128, height: 32)
    {
        Options =
        {
            // Preset options you can tweak:
            Background = DmdColor.Transparent,
            DefaultTextColor = new DmdColor(255, 180, 40, 255),
            DefaultBlend = DmdBlendMode.Max
        }
    };

    // Try loading sample sprites if present (optional).
    engine.TryLoadSampleSprites();

    return engine;
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Switch renderer by query: ?mode=pinball or ?mode=flat
app.MapGet("/frame.png", (HttpContext ctx, DmdEngine engine) =>
{
    var mode = (ctx.Request.Query["mode"].ToString() ?? "").ToLowerInvariant();
    var scale = int.TryParse(ctx.Request.Query["scale"], out var s) ? Math.Clamp(s, 1, 20) : 8;

    // Advance engine based on wall clock (simple, stateless)
    // In real use you may run a timer and cache the last frame.
    engine.Step();

    var canvas = engine.Canvas;

    IDmdOutputRenderer renderer = mode == "flat"
        ? new FlatPixelRenderer()
        : new PinballDotsRenderer();

    var png = renderer.RenderPng(canvas, scale);
    return Results.File(png, "image/png");
});

app.MapGet("/api/text", (DmdEngine engine, string text) =>
{
    // Very simple: set the scroller text
    engine.SetScrollingText(text);
    return Results.Ok(new { ok = true, text });
});

app.MapGet("/health", () => Results.Ok(new { ok = true, time = DateTimeOffset.Now }));

app.Run();

