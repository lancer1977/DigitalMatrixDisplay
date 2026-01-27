namespace DmdSkiaFlex.Core;

public interface IDmdCanvas
{
    int Width { get; }
    int Height { get; }

    void Clear(DmdColor? color = null);

    void SetPixel(int x, int y, DmdPixel px, DmdBlendMode mode = DmdBlendMode.Max);

    void DrawSprite(
        IDmdSprite sprite,
        int x,
        int y,
        DmdBlendMode mode = DmdBlendMode.Max,
        float opacity = 1f,
        byte intensityScale = 255);

    void DrawText(string text, int x, int y, DmdColor color, byte intensity = 255);
    int MeasureText(string text);
}
