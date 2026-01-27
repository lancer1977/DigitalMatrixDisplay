using DmdSkiaFlex.Core;

namespace DmdSkiaFlex.Sprites;

public record Palette(byte Threshold, DmdColor Color)
{
    public static Palette Create() => new Palette(255, DmdColor.White);
};

//public sealed class PaletteSprite : IDmdSprite
//{
//    private readonly IDmdSprite _src;
//    private readonly Palette[] _palette;

//    public int Width => _src.Width;
//    public int Height => _src.Height;

//    public PaletteSprite(IDmdSprite src, params Palette[] palette)
//    {
//        _src = src;
//        _palette = palette.OrderBy(p => p.Threshold).ToArray();
//        if (_palette.Length == 0)
//            _palette = [Palette.Create()];
//    }

//    public DmdPixel GetPixel(int x, int y)
//    {
//        var p = _src.GetPixel(x, y);
//        if (p.Intensity == 0) return p;

//        var color = _palette[^1].Color;
//        foreach (var (t, c) in _palette)
//        {
//            if (p.Intensity > t)
//            {
//                continue;
//            }

//            color = c;
//        }

//        return p with { Color = color };
//    }
//}
