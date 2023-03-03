using Games.GrumpyBear.Core.SaveSystem;
using UnityEngine;

namespace DualityGame.Iteractables
{
    [RequireComponent(typeof(SaveableEntity))]
    public abstract class InteractableBase : MonoBehaviour, IInteractable, ISaveableComponent
    {
        [Header("Interaction prompt")]
        [SerializeField] public string _prompt;
        [SerializeField] public Vector3 _promptOffset = new Vector3(0, 1.5f, 0);
        
        [Header("Interaction")]
        [SerializeField] private bool _onlyTriggerOnce = true;

        public string Prompt => _hasTriggered ? null : _prompt;
        public Vector3 PromptPosition => transform.position + _promptOffset;

        private bool _hasTriggered;

        public void Interact(GameObject actor)
        {
            if (_hasTriggered) return;
            if (_onlyTriggerOnce) _hasTriggered = true;
            Trigger(actor);
        }

        protected abstract void Trigger(GameObject actor);

        #region ISaveableComponent
        object ISaveableComponent.CaptureState() => _hasTriggered;
        void ISaveableComponent.RestoreState(object state) => _hasTriggered = (bool)state;
        #endregion
    }
}
