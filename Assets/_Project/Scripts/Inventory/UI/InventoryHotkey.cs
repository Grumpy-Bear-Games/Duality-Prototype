using DualityGame.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DualityGame.Inventory.UI
{
    public class InventoryHotkey : MonoBehaviour
    {
        [SerializeField] private InputActionReference _input;

        private void Awake()
        {
            _input.action.Enable();
            _input.action.performed += OnInventoryToggle;
        }
        
        private void OnDestroy()
        {
            _input.action.Disable();
            _input.action.performed -= OnInventoryToggle;
        }

        private static void OnInventoryToggle(InputAction.CallbackContext obj) => PlayState.Current.Set(PlayState.Current.Value == PlayState.State.Inventory ? PlayState.State.Moving : PlayState.State.Inventory);
    }
}