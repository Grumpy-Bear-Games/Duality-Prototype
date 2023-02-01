using System.Collections;
using UnityEngine;

namespace DualityGame.VFX
{
    public abstract class ScreenFaderProviderBase : MonoBehaviour, IScreenFaderProvider
    {
        [SerializeField] protected ScreenFader _trigger;
        
        protected virtual void OnEnable() => _trigger.RegisterVFX(this);
        protected virtual void OnDisable() => _trigger.Unregister(this);

        public abstract IEnumerator Execute(ScreenFader.Direction direction);
    }
}
