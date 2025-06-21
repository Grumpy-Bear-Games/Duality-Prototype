using System;
using Games.GrumpyBear.Core.SaveSystem;
using UnityEngine;

namespace DualityGame.Warp
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteController : MonoBehaviour, ISaveableComponent
    {
        private static readonly int OutlineEnabledProperty = Shader.PropertyToID("_Outline_Enabled");

        [SerializeField] private bool _outline = true;

        private SpriteRenderer _spriteRenderer;
        private MaterialPropertyBlock _mpb;

        public bool Outline
        {
            get => _outline;
            set {
                _outline = value;
                UpdateMaterialPropertyBlock();
            }
        }

        private void UpdateMaterialPropertyBlock()
        {
            _mpb.Clear();
            _mpb.SetFloat(OutlineEnabledProperty, _outline ? 1 : 0);
            _spriteRenderer.SetPropertyBlock(_mpb);
        }

        public void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _mpb = new MaterialPropertyBlock();
        }

        private void Start() => UpdateMaterialPropertyBlock();

        #region ISavableComponent
        object ISaveableComponent.CaptureState() => _outline;
        void ISaveableComponent.RestoreState(object state) => Outline = (bool)state;
        #endregion
    }
}
