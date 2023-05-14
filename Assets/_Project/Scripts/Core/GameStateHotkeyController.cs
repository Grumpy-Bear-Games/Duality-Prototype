using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DualityGame.Core
{
    public class GameStateHotkeyController : MonoBehaviour
    {
        [SerializeField] private List<Transition> _transitions = new();

        private readonly Dictionary<InputAction, Dictionary<GameState, GameState>> _transitionLookup = new();
        private void Awake()
        {
            foreach (var transition in _transitions)
            {
                if (!_transitionLookup.TryGetValue(transition.Hotkey, out var gameStateTransition))
                {
                    gameStateTransition = new Dictionary<GameState, GameState>();
                    _transitionLookup.Add(transition.Hotkey, gameStateTransition);
                }
                gameStateTransition.Add(transition.From, transition.To);
            }

            foreach (var transitionHotkey in _transitionLookup.Keys)
            {
                transitionHotkey.performed += OnHotkeyTriggered;
            }
        }

        private void OnEnable()
        {
            foreach (var transitionHotkey in _transitionLookup.Keys)
            {
                transitionHotkey.Enable();
            }
        }

        private void OnDisable()
        {
            foreach (var transitionHotkey in _transitionLookup.Keys)
            {
                transitionHotkey.Disable();
            }
        }
        
        private void OnDestroy()
        {
            foreach (var transitionHotkey in _transitionLookup.Keys)
            {
                transitionHotkey.performed -= OnHotkeyTriggered;
            }
        }

        private void OnHotkeyTriggered(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed) return;
            if (!_transitionLookup.TryGetValue(ctx.action, out var gameStateTransition)) return;
            if (!gameStateTransition.TryGetValue(GameState.Current, out var newGameState)) return;
            
            newGameState.SetActive();
        }

        [Serializable]
        public class Transition
        {
            [field: SerializeField] public InputActionReference Hotkey { get; private set;  }
            [field: SerializeField] public GameState From { get; private set;  }
            [field: SerializeField] public GameState To { get; private set;  }
        }
    }
}
