using System;
using UnityEngine;

namespace DualityGame.Iteractables
{
    public class InteractController : MonoBehaviour
    {
        [SerializeField] private float _radius = 3f;
        [SerializeField] private InteractableObservable _closestInteractable;


        private void Awake() => Realm.Realm.Subscribe(UpdateEnabled);
        private void OnDestroy() => Realm.Realm.Unsubscribe(UpdateEnabled);
        private void UpdateEnabled(Realm.Realm realm) => enabled = realm != null;

        private void FixedUpdate()
        {
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

        private void OnInteract() => _closestInteractable.Value?.Interact(gameObject);
    }
}
