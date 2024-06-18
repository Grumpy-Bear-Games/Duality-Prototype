using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace DualityGame.Warp
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteController : MonoBehaviour
    {
        [SerializeField] private bool _outline;

        private SpriteRenderer _spriteRenderer;

        public bool Outline
        {
            get => _outline;
            set {
                _outline = value;
            }
        }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            //if (EditorApplication.isPlayingOrWillChangePlaymode) ApplyMaterialProperties();
        }

        private void Reset() => _spriteRenderer = GetComponent<SpriteRenderer>();
        #endif
    }
}
