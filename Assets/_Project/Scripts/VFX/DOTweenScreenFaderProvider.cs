using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace DualityGame.VFX
{
    public abstract class DOTweenScreenFaderProvider : ScreenFaderProviderBase, IScreenFaderProvider
    {
        [SerializeField] protected float _duration = 2f;
        [SerializeField] protected bool _initiallyFadedIn;

        protected virtual void Start() => Setter(_initiallyFadedIn ? 1f : 0f);

        protected abstract void Setter(float weight);

        private IEnumerator FadeOut()
        {
            yield return DOTween.To(Setter, 0f, 1f, _duration)
                .WaitForCompletion();
        }

        private IEnumerator FadeIn()
        {
            yield return DOTween.To(Setter, 1f, 0f, _duration)
                .WaitForCompletion();
        }

        public override IEnumerator Execute(ScreenFader.Direction direction)
        {
            yield return direction switch
            {
                ScreenFader.Direction.FadeIn => FadeIn(),
                ScreenFader.Direction.FadeOut => FadeOut(),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
    }
}
