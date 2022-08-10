using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace DualityGame.UI
{
    public class AlternativeSubmitHandler : MonoBehaviour
    {
        [Header("Inputs")]
        [SerializeField] private InputActionReference _submit;
        [SerializeField] private InputActionReference _cancel;

        private EventSystem _eventSystem;

        private void Awake()
        {
            _submit.action.performed += Submit;
            _cancel.action.performed += Cancel;
            _eventSystem = EventSystem.current;
        }

        private void Cancel(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            var currentGO = _eventSystem.currentSelectedGameObject;
            if (currentGO == null) return;

            ExecuteEvents.ExecuteHierarchy(currentGO, new BaseEventData(_eventSystem), ExecuteEvents.cancelHandler);
        }

        private void Submit(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            var currentGO = _eventSystem.currentSelectedGameObject;
            if (currentGO == null) return;

            ExecuteEvents.Execute(currentGO, new BaseEventData(_eventSystem), ExecuteEvents.submitHandler);
        }

        private void OnEnable()
        {
            _submit.action.Enable();
            _cancel.action.Enable();
        }
        
        private void OnDisable()
        {
            _submit.action.Disable();
            _cancel.action.Disable();
        }
    }
}
