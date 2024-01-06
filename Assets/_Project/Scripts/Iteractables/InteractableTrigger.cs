using Games.GrumpyBear.Core.SaveSystem;
using UnityEngine;
using UnityEngine.Events;

namespace DualityGame.Iteractables
{
    [RequireComponent(typeof(SaveableEntity))]
    public class InteractableTrigger : Interactable, ISaveableComponent
    {
        [SerializeField] private UnityEvent<GameObject> _onInteract;
        [SerializeField] private IInteractable.InteractionType _type = IInteractable.InteractionType.Touch;

        [Header("Interaction")]
        [SerializeField] private bool _onlyTriggerOnce = true;
        
        public override bool Enabled => base.Enabled && !_hasTriggered;

        public override IInteractable.InteractionType Type => _type;

        private bool _hasTriggered;
        
        public override void Interact(GameObject actor)
        {
            if (_hasTriggered) return;
            if (_onlyTriggerOnce) _hasTriggered = true;
            _onInteract.Invoke(actor);
        }

        #region ISaveableComponent
        object ISaveableComponent.CaptureState() => _hasTriggered;
        void ISaveableComponent.RestoreState(object state) => _hasTriggered = (bool)state;
        #endregion
    }
}
