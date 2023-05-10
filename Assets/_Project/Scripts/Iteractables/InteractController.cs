using UnityEngine;
using UnityEngine.InputSystem;

namespace DualityGame.Iteractables
{
    public class InteractController : MonoBehaviour
    {
        [SerializeField] private InputActionReference _interactAction;
        [SerializeField] private float _radius = 3f;
        [SerializeField] private InteractableObservable _closestInteractable;

        private void Awake() => _interactAction.action.performed += _ => _closestInteractable.Value?.Interact(gameObject);

        private void OnEnable() => _interactAction.action.Enable();
        private void OnDisable() => _interactAction.action.Disable();

        private void FixedUpdate()
        {
            if (Realm.Realm.Current == null) return;
            
            IInteractable closest = null;
            var closestDistance = Mathf.Infinity;
            foreach (var collider in Physics.OverlapSphere(transform.position, _radius, Realm.Realm.Current.LevelLayerMask))
            {
                var interactable = collider.GetComponent<IInteractable>();
                if (interactable == null) continue;
                if (interactable.Prompt == null) continue;
                var distance = Vector3.Distance(transform.position, collider.transform.position);
                if (closest != null && distance > closestDistance) continue;
                closest = interactable;
                closestDistance = distance;
            }

            _closestInteractable.Set(closest);
        }
    }
}
