using UnityEngine;

namespace DualityGame.Iteractables
{
    public abstract class InteractableBase : MonoBehaviour, IInteractable
    {
        [Header("Interaction prompt")]
        [SerializeField] public string _prompt;
        [SerializeField] public Vector3 _promptOffset;
        
        [Header("Interaction")]
        [SerializeField] private bool _onlyTriggerOnce;

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
    }
}