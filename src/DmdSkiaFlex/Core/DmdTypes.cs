namespace DmdSkiaFlex.Core;

public readonly record struct DmdColor(byte R, byte G, byte B, byte A = 255)
{
    public static readonly DmdColor Transparent = new(0, 0, 0, 0);
    public static readonly DmdColor White = new(255, 255, 255, 255);
    public static readonly DmdColor Amber = new(255, 140, 0, 255);
}

public readonly record struct DmdPixel(byte Intensity, DmdColor Color)
{
    public static readonly DmdPixel Off = new(0, DmdColor.Transparent);
}

public enum DmdBlendMode { Max, 
    Alpha, 
    Additive }
