using DualityGame.VFX;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DualityGame.Iteractables
{
    public class InteractController : MonoBehaviour
    {
        [SerializeField] private InputActionReference _interactAction;
        [SerializeField] private InteractableObservable _closestInteractable;

        private void Awake() => _interactAction.action.performed += OnInteract;
        private void OnEnable() => _interactAction.action.Enable();
        private void OnDisable() => _interactAction.action.Disable();
        private void OnDestroy() => _interactAction.action.performed -= OnInteract;

        private void Update() => ShaderGlobals.PlayerPosition = transform.position;

        private void OnInteract(InputAction.CallbackContext _) => _closestInteractable.Value?.Interact(gameObject);
    }
}
