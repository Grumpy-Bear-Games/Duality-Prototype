using UnityEngine;

namespace DualityGame.Utilities
{
    public class FloatingIsland : MonoBehaviour
    {
        [SerializeField] private float _motionScale = 5.0f;
        [SerializeField] private float _timeScale = 0.1f;

        private Vector3 _originalPosition;

        private void Awake() => _originalPosition = transform.position;

        void Update()
        {
            transform.position = _originalPosition +
                                 Mathf.Sin(Time.realtimeSinceStartup * _timeScale) * _motionScale * Vector3.up;

        }
    }
}
