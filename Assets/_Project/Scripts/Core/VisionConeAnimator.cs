using DG.Tweening;
using UnityEngine;

namespace DualityGame.Core
{
    public class VisionConeAnimator : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private VisionCone _visionCone;

        [Header("Animation parameters")]
        [SerializeField][Min(0f)] private float _duration = 0.5f;
        [SerializeField] private Ease _ease = Ease.InOutCubic;
        
        [Header("Patrolling")]
        [SerializeField][ColorUsage(true, true)] private Color _patrolColor;
        [SerializeField] private float _patrolRange;
        [SerializeField] private float _patrolFOV;
        
        [Header("Chasing")]
        [SerializeField][ColorUsage(true, true)] private Color _chaseColor;
        [SerializeField] private float _chaseRange;
        [SerializeField] private float _chaseFOV;

#if UNITY_EDITOR
        [Header("Preview")]
        [SerializeField][Range(0f, 1f)] private float _previewValue = 0f;
#endif

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) return;
            UpdateVisionCone(_previewValue);
        }
#endif
        
        private MaterialPropertyBlock _materialPropertyBlock;
        private static readonly int ColorProperty = Shader.PropertyToID("_Color");
        private Tweener _tween;

        private EnemyState _state = EnemyState.Patrolling;
        public EnemyState State
        {
            get => _state;
            set
            {
                if (_state == value) return;
                _state = value;
                UpdateTween();
            }
        }

        private void UpdateTween()
        {
            if (_tween == null) return;
            var isPatrolling = _state == EnemyState.Patrolling;
            if (_tween.isBackwards == isPatrolling) return;
            _tween.Flip();
            if (!_tween.IsPlaying()) _tween.Play();
        }

        private void Awake()
        {
            InitializeTween();
            UpdateVisionCone(0f);
        }

        private void OnDestroy() => _tween.Kill();

        private void InitializeTween()
        {
            _tween = DOTween.To(UpdateVisionCone, 0f, 1f, _duration)
                .SetEase(_ease)
                .SetAutoKill(false)
                .Pause();
            _tween.Flip();
        }

        private void UpdateVisionCone(float value)
        {
            _materialPropertyBlock ??= new MaterialPropertyBlock();
            _materialPropertyBlock.SetColor(ColorProperty, Color.Lerp(_patrolColor, _chaseColor, value));
            _renderer.SetPropertyBlock(_materialPropertyBlock);

            _visionCone.Range = Mathf.Lerp(_patrolRange, _chaseRange, value);
            _visionCone.FOV = Mathf.Lerp(_patrolFOV, _chaseFOV, value);
        }

        public enum EnemyState
        {
            Patrolling, Chasing
        }
    }
}
