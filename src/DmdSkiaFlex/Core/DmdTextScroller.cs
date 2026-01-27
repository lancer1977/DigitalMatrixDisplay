namespace DmdSkiaFlex.Core;

public sealed class DmdTextScroller
{
    public string Text { get; set; } = "";
    public int Y { get; set; } = 0;
    public int SpeedPxPerSec { get; set; } = 30;
    public DmdColor Color { get; set; } = DmdColor.White;
    public byte Intensity { get; set; } = 255;

    private float _x;

    public void Reset(int startX) => _x = startX;

    public void Update(float dtSeconds) => _x -= SpeedPxPerSec * dtSeconds;

    public void Render(IDmdCanvas canvas)
        => canvas.DrawText(Text, (int)_x, Y, Color, Intensity);

    public void WrapIfNeeded(IDmdCanvas canvas)
    {
        var w = canvas.MeasureText(Text);
        if (_x < -w) _x = canvas.Width;
    }
}
