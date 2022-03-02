using UnityEngine;
using UnityEngine.Rendering;

namespace DualityGame.VFX
{
    [RequireComponent(typeof(Volume))]
    public class VolumeFader : ScreenFaderProviderBase
    {
        private Volume _volume;
        private void Awake() => _volume = GetComponent<Volume>();
        protected override void Setter(float weight) => _volume.weight = weight;
    }
}
