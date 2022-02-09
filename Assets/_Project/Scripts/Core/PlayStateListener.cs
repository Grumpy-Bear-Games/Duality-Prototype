using System;
using UnityEngine;
using UnityEngine.Events;

namespace DualityGame.Core
{
    public class PlayStateListener : MonoBehaviour
    {
        [SerializeField] private PlayState _playState;
        [SerializeField] private UnityEvent<PlayState.State> _onChange;
        [SerializeField] private UnityEvent _onMoving;
        [SerializeField] private UnityEvent _onTalking;

        private void OnChange(PlayState.State playState)
        {
            _onChange.Invoke(playState);
            switch (playState)
            {
                case PlayState.State.Moving:
                    _onMoving.Invoke();
                    break;
                case PlayState.State.Talking:
                    _onTalking.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(playState), playState, null);
            }
        }

        private void OnEnable() => _playState.Subscribe(OnChange);
        private void OnDisable() => _playState.Unsubscribe(OnChange);
    }
}
