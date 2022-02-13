using System.Collections;

namespace DualityGame.VFX
{
    public interface IVFXFadeEffect
    {
        IEnumerator Execute(Direction direction);

        public enum Direction
        {
            FadeIn, FadeOut
        }
    }
}
