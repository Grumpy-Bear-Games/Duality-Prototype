using DualityGame.Realm;
using UnityEngine;
using UnityEngine.Events;

namespace DualityGame.Iteractables
{
    public class InteractController : MonoBehaviour
    {
        [SerializeField] private RealmManager _realmManager;
        [SerializeField] private float _radius = 3f;
        [SerializeField] private UnityEvent _onUse;

        private IInteractable _closestInteractable = null;
        
        private void FixedUpdate()
        {
            IInteractable closest = null;
            var closestDistance = Mathf.Infinity;
            foreach (var collider in Physics.OverlapSphere(transform.position, _radius, 1 << _realmManager.CurrentRealm.Value.LevelLayer))
            {
                var interactable = collider.GetComponent<IInteractable>();
                if (interactable == null) continue;
                var distance = Vector3.Distance(transform.position, collider.transform.position);
                if (closest != null && distance > closestDistance) continue;
                closest = interactable;
                closestDistance = distance;
            }

            if (_closestInteractable == closest) return;
            
            _closestInteractable = closest;
        }

        private void OnInteract()
        {
            if (_closestInteractable != null)
            {
                _closestInteractable.Interact(gameObject);
            }
            else
            {
                _onUse.Invoke();
            }
        }
    }
}
