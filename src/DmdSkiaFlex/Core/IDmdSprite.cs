namespace DmdSkiaFlex.Core;

public interface IDmdSprite
{
    int Width { get; }
    int Height { get; }

    DmdPixel GetPixel(int x, int y);
}
