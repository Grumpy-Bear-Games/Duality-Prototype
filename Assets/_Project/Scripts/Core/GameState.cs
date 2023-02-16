using System;
using Games.GrumpyBear.Core.Observables;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace DualityGame.Core
{
    [CreateAssetMenu(menuName = "Duality/Game State", fileName = "Game state", order = 0)]
    public class GameState : ScriptableObject
    {
        
        private static readonly Observable<GameState> _current = new();
        public static IReadonlyObservable<GameState> Current => _current;

        [SerializeField] private bool _initialState = false;

        public event Action OnEnter;
        public event Action OnLeave;

        public bool IsActive => _current.Value == this;
        
        public void SetActive()
        {
            if (_current.Value == this) return;
            _current.Value.OnLeave?.Invoke();
            _current.Set(this);
            _current.Value.OnEnter?.Invoke();
        }

        private void OnEnable()
        {
            #if UNITY_EDITOR
            if (!EditorApplication.isPlayingOrWillChangePlaymode) return;
            #endif
            if (!_initialState) return;
            Debug.Assert(_current.Value == null, $"The can only be one initial state. Current initial state is {_current.Value}", this);
            _current.Set(this);
        }
    }
}
