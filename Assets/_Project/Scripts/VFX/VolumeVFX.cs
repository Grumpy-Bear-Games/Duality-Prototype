using UnityEngine;
using UnityEngine.Rendering;

namespace DualityGame.VFX
{
    [RequireComponent(typeof(Volume))]
    public class VolumeVFX : VFXFadeBase
    {
        private Volume _volume;
        private void Awake() => _volume = GetComponent<Volume>();
        protected override void Setter(float weight) => _volume.weight = weight;
    }
}
