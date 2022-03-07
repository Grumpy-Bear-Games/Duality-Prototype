using System;
using System.Collections;
using UnityEngine;

namespace DualityGame.VFX
{
    [CreateAssetMenu(fileName = "Screen Fader", menuName = "Duality/Screen Fader", order = 0)]
    public class ScreenFader : ScriptableObject
    {
        [Tooltip("What happens if no provider is registered")]
        [SerializeField] private NullAction _defaultNullAction;
        private IScreenFaderProvider _provider;

        public void RegisterVFX(IScreenFaderProvider provider) => _provider = provider;

        public void Unregister(IScreenFaderProvider vfx)
        {
            if (_provider == vfx) _provider = null;
        }

        public IEnumerator Execute(Direction direction, NullAction nullAction)
        {
            switch (nullAction)
            {
                case NullAction.Wait:
                    while (_provider == null)
                    {
                        yield return null;
                    }
                    break;
                case NullAction.Ignore:
                    if (_provider == null) yield break;
                    break;
                case NullAction.Exception:
                    if (_provider == null) throw new NullReferenceException("No ScreenFaderProvider registered");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            yield return _provider.Execute(direction);
        }

        public IEnumerator Execute(Direction direction) => Execute(direction, _defaultNullAction);

        public IEnumerator Wrap(Action runInBetween, NullAction nullAction)
        {
            yield return Execute(Direction.FadeOut, nullAction);
            runInBetween();
            yield return Execute(Direction.FadeIn, nullAction);
        }

        public IEnumerator Wrap(Action runInBetween) => Wrap(runInBetween, _defaultNullAction);

        public IEnumerator Wrap(IEnumerator runInBetween, NullAction nullAction)
        {
            yield return Execute(Direction.FadeOut, nullAction);
            yield return runInBetween;
            yield return Execute(Direction.FadeIn, nullAction);
        }

        public IEnumerator Wrap(IEnumerator runInBetween) => Wrap(runInBetween, _defaultNullAction);

        public enum Direction
        {
            FadeIn, FadeOut
        }
        
        public enum NullAction
        {
            Wait, Ignore, Exception
        }
    }
}
