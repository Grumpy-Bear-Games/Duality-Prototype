using UnityEngine;
using UnityEngine.Events;

namespace DualityGame.Utilities
{
    public class UnityLifetimeEvents : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onAwake;
        [SerializeField] private UnityEvent _onEnable;
        [SerializeField] private UnityEvent _onDisable;
        [SerializeField] private UnityEvent _onDestroy;

        private void Awake() => _onAwake.Invoke();
        private void OnEnable() => _onEnable.Invoke();
        private void OnDisable() => _onDisable.Invoke();
        private void OnDestroy() => _onDestroy.Invoke();
    }
}
