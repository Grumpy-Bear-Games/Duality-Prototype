using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace DualityGame.Realm
{
    [RequireComponent(typeof(Volume))]
    public class VolumeEffect : MonoBehaviour
    {
        [SerializeField] private float duration = 2f;
        private Volume _volume;

        private void Awake()
        {
            _volume = GetComponent<Volume>();
            _volume.weight = 0f;
        }

        public IEnumerator FadeOut()
        {
            _volume.weight = 0f;
            while (_volume.weight < 1f)
            {
                _volume.weight += Time.deltaTime / duration;
                yield return null;
            }
        }
        
        public IEnumerator FadeIn()
        {
            _volume.weight = 1f;
            while (_volume.weight > 0f)
            {
                _volume.weight -= Time.deltaTime / duration;
                yield return null;
            }
        }
    }
}
