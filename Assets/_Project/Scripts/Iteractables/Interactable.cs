using System.Collections.Generic;
using UnityEngine;

namespace DualityGame.Iteractables
{
    public abstract class Interactable : MonoBehaviour, IInteractable
    {
        private static readonly HashSet<IInteractable> _runtimeSet = new();
        public static IReadOnlyCollection<IInteractable> Interactables => _runtimeSet;
        
        [Header("Interaction prompt")]
        [SerializeField] public Vector3 _promptOffset = new(0, 1.5f, 0);

        protected void OnEnable() => _runtimeSet.Add(this);

        private void OnDisable() => _runtimeSet.Remove(this);

        public Vector3 Position => transform.position;

        public abstract void Interact(GameObject actor);
        public Vector3 PromptPosition => transform.position + _promptOffset;
        public virtual bool Enabled => Realm.Realm.Current && gameObject.layer == Realm.Realm.Current.LevelLayer;

        public abstract IInteractable.InteractionType Type { get; }


        public void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(PromptPosition, 0.1f);
        }

    }
}
