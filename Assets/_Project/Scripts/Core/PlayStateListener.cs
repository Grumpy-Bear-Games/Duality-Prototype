using UnityEngine;
using UnityEngine.Events;

namespace DualityGame.Core
{
    public class PlayStateListener : MonoBehaviour
    {
        [SerializeField] private PlayState.State _state;
        [SerializeField] private UnityEvent _onEnter;
        [SerializeField] private UnityEvent _onLeave;

        private void OnChange(PlayState.State playState) => (playState == _state ? _onEnter : _onLeave)?.Invoke();

        private void OnEnable() => PlayState.Current.Subscribe(OnChange);
        private void OnDisable() => PlayState.Current.Unsubscribe(OnChange);

    }
}
