using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace DualityGame.VFX
{
    public abstract class ScreenFaderProviderBase : MonoBehaviour, IScreenFaderProvider
    {
        [SerializeField] protected VFX.ScreenFader _trigger;
        [SerializeField] protected float _duration = 2f;
        [SerializeField] protected bool _initiallyFadedIn;

        protected virtual void Start() => Setter(_initiallyFadedIn ? 1f : 0f);

        protected virtual void OnEnable() => _trigger.RegisterVFX(this);

        protected virtual void OnDisable() => _trigger.Unregister(this);

        protected abstract void Setter(float weight);

        protected virtual IEnumerator FadeOut()
        {
            yield return DOTween.To(Setter, 0f, 1f, _duration)
                .WaitForCompletion();
        }

        protected virtual IEnumerator FadeIn()
        {
            yield return DOTween.To(Setter, 1f, 0f, _duration)
                .WaitForCompletion();
        }

        IEnumerator IScreenFaderProvider.Execute(VFX.ScreenFader.Direction direction)
        {
            yield return direction switch
            {
                VFX.ScreenFader.Direction.FadeIn => FadeIn(),
                VFX.ScreenFader.Direction.FadeOut => FadeOut(),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
    }
}
