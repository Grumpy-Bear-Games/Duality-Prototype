using UnityEngine;

namespace DualityGame.Iteractables
{
    public class InteractController : MonoBehaviour
    {
        [SerializeField] private float _radius = 3f;
        [SerializeField] private InteractableObservable _closestInteractable;

        private Realm.Realm _realm;
        
        public void SetRealm(Realm.Realm realm)
        {
            _realm = realm;
            enabled = _realm != null;
        }

        private void Awake() => enabled = _realm != null;

        private void FixedUpdate()
        {
            IInteractable closest = null;
            var closestDistance = Mathf.Infinity;
            foreach (var collider in Physics.OverlapSphere(transform.position, _radius, _realm.LevelLayerMask))
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

        private void OnInteract() => _closestInteractable.Value?.Interact(gameObject);
    }
}
