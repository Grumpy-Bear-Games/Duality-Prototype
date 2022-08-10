using System.Collections;

namespace DualityGame.VFX
{
    public interface IScreenFaderProvider
    {
        IEnumerator Execute(ScreenFader.Direction direction);
    }
}
