using DualityGame.Realm;
using Games.GrumpyBear.Core.Observables;
using UnityEngine;
using UnityEngine.Events;

namespace DualityGame.Iteractables
{
    public class InteractController : MonoBehaviour
    {
        [SerializeField] private float _radius = 3f;
        [SerializeField] private UnityEvent _onUse;
        [SerializeField] private RealmObservable _realm;
        [SerializeField] private InteractableObservable _closestInteractable;

        private void FixedUpdate()
        {
            IInteractable closest = null;
            var closestDistance = Mathf.Infinity;
            foreach (var collider in Physics.OverlapSphere(transform.position, _radius, _realm.Value.LevelLayerMask))
            {
                var interactable = collider.GetComponent<IInteractable>();
                if (interactable == null) continue;
                var distance = Vector3.Distance(transform.position, collider.transform.position);
                if (closest != null && distance > closestDistance) continue;
                closest = interactable;
                closestDistance = distance;
            }

            _closestInteractable.Set(closest);
        }

        private void OnInteract()
        {
            if (_closestInteractable.Value != null) {
                _closestInteractable.Value.Interact(gameObject);
            } else {
                _onUse.Invoke();
            }
        }
    }
}
