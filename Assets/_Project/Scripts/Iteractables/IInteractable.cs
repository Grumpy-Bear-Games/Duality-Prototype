using UnityEngine;

namespace DualityGame.Iteractables
{
    public interface IInteractable
    {
        Vector3 Position { get; }
        void Interact(GameObject actor);
        Vector3 PromptPosition { get; }
        bool Enabled { get;  }
        InteractionType Type { get; }

        public enum InteractionType
        {
            Talk,
            Touch,
            View,
            Doorway,
        }
    }
}
