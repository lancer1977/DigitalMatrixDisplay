using DmdSkiaFlex.Core;

namespace DmdSkiaFlex.Rendering;

public interface IDmdOutputRenderer
{
    byte[] RenderPng(DmdCanvas canvas, int scale);
}
