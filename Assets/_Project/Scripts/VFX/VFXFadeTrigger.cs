using System;
using System.Collections;
using UnityEngine;

namespace DualityGame.VFX
{
    [CreateAssetMenu(fileName = "VFX Manager", menuName = "Duality/VFX Manager", order = 0)]
    public class VFXFadeTrigger : ScriptableObject
    {
        [Tooltip("What happens if no VFX is registered")]
        [SerializeField] private NullAction _nullAction;
        private IVFXFadeEffect _vfx;

        public void RegisterVFX(IVFXFadeEffect vfx) => _vfx = vfx;

        public void Unregister(IVFXFadeEffect vfx)
        {
            if (_vfx == vfx) _vfx = null;
        }

        public IEnumerator Execute(IVFXFadeEffect.Direction direction)
        {
            switch (_nullAction)
            {
                case NullAction.Wait:
                    while (_vfx == null)
                    {
                        yield return null;
                    }
                    break;
                case NullAction.Ignore:
                    if (_vfx == null) yield break;
                    break;
                case NullAction.Exception:
                    if (_vfx == null) throw new NullReferenceException("No VFX registered");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            yield return _vfx.Execute(direction);
        }
    }
}
