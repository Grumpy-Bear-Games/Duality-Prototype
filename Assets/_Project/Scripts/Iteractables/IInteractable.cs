using UnityEngine;

namespace DualityGame.Iteractables
{
    public interface IInteractable
    {
        void Interact(GameObject actor);
        string Prompt { get; }
        Vector3 PromptPosition { get; }
    }
}
