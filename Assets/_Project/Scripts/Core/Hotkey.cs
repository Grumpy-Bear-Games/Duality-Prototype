using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace DualityGame.Core
{
    public class Hotkey : MonoBehaviour
    {
        [SerializeField] private InputActionReference _input;
        [SerializeField] private UnityEvent _onPerform;


        private void Awake() => _input.action.Enable();
        private void OnEnable() => _input.action.performed += OnPerformed;
        private void OnDisable() => _input.action.performed -= OnPerformed;
        private void OnDestroy() => _input.action.Disable();

        private void OnPerformed(InputAction.CallbackContext _) => _onPerform.Invoke();
    }
}
