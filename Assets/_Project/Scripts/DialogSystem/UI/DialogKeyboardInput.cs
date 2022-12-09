using UnityEngine;
using UnityEngine.InputSystem;

namespace DualityGame.DialogSystem.UI
{
    [RequireComponent(typeof(DialogUI))]
    public class DialogKeyboardInput : MonoBehaviour
    {
        [SerializeField] private InputActionReference _selectOptionInputActionReference;

        private DialogUI _dialogUI;
		
        private void Awake()
        {
            _dialogUI = GetComponent<DialogUI>();
            _selectOptionInputActionReference.action.performed += SelectOptionFromKeyboard;
        }

        private void OnDestroy() => _selectOptionInputActionReference.action.performed -= SelectOptionFromKeyboard;

        private void OnEnable() => _selectOptionInputActionReference.action.Enable();

        private void OnDisable() => _selectOptionInputActionReference.action.Disable();

        private void SelectOptionFromKeyboard(InputAction.CallbackContext ctx)
        {
            if (int.TryParse(ctx.control.name, out var value))
            {
                _dialogUI.SelectOption(value);	
            }
        }
		
    }
}