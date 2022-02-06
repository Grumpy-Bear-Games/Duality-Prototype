using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace DualityGame.Realm
{
    [RequireComponent(typeof(Volume))]
    public class VolumeEffect : MonoBehaviour
    {
        [SerializeField] private float duration = 2f;
        [SerializeField] private AnimationCurve _animationCurve;
        private Volume _volume;

        private void Awake()
        {
            _volume = GetComponent<Volume>();
            _volume.weight = 0f;
        }

        public IEnumerator FadeOut()
        {
            _volume.weight = 0f;
            var time = 0f;
            while (time < 1f)
            {
                time += Time.deltaTime / duration;
                _volume.weight = _animationCurve.Evaluate(time);
                yield return null;
            }
        }
        
        public IEnumerator FadeIn()
        {
            _volume.weight = 1f;
            var time = 1f;
            while (time > 0f)
            {
                time -= Time.deltaTime / duration;
                _volume.weight = _animationCurve.Evaluate(time);
                yield return null;
            }
        }
    }
}
