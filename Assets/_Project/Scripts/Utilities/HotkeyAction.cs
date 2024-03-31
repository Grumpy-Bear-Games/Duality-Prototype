using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace DualityGame.Utilities
{
    public class HotkeyAction : MonoBehaviour
    {
        [SerializeField] private InputActionReference _inputAction;
        [SerializeField] private UnityEvent _action;

        private void Awake()
        {
            _inputAction.action.performed += PerformAction;
            _inputAction.action.Enable();
        }

        private void OnDestroy()
        {
            _inputAction.action.Disable();
            _inputAction.action.performed -= PerformAction;
        }

        private void PerformAction(InputAction.CallbackContext obj) => _action?.Invoke();
    }
}