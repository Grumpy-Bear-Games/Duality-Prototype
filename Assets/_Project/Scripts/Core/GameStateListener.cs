using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DualityGame.Core
{
    public class GameStateListener : MonoBehaviour
    {
        [SerializeField] private GameState _state;
        [SerializeField] private List<MonoBehaviour> _enabledWhenInState = new();
        [SerializeField] private UnityEvent _onEnter;
        [SerializeField] private UnityEvent _onLeave;


        private void Awake()
        {
            _state.OnEnter += OnEnter;
            _state.OnLeave += OnLeave;
        }

        private void Start()
        {
            (_state.IsActive ? _onEnter : _onLeave).Invoke();
            SetComponentsEnabled(_state.IsActive);
        }

        private void OnDestroy()
        {
            _state.OnEnter -= OnEnter;
            _state.OnLeave -= OnLeave;
        }

        private void OnEnter()
        {
            _onEnter.Invoke();
            SetComponentsEnabled(true);
        }

        private void OnLeave()
        {
            _onLeave.Invoke();
            SetComponentsEnabled(false);
        }

        private void SetComponentsEnabled(bool enable) => _enabledWhenInState.ForEach(c=>c.enabled=enable);
    }
}
