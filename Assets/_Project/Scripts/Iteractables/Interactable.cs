using UnityEngine;
using UnityEngine.Events;

namespace DualityGame.Iteractables
{
    public class Interactable : InteractableBase
    {
        [SerializeField] private UnityEvent<GameObject> _onInteract;

        protected override void Trigger(GameObject actor) => _onInteract.Invoke(actor);
    }
}
