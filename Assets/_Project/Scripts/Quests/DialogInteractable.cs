using DualityGame.Iteractables;
using NodeCanvas.DialogueTrees;
using UnityEngine;

namespace DualityGame.Quests
{
    public class DialogInteractable : MonoBehaviour, IInteractable
    {
        private DialogueTreeController _controller;
        private void Awake() => _controller = GetComponent<DialogueTreeController>();
        public void Interact(GameObject actor)
        {
            var instigator = actor.GetComponent<DialogueActor>();
            if (instigator != null)
            {
                _controller.StartDialogue(instigator);
            }
            else
            {
                _controller.StartDialogue();
            }
        }

        public string Prompt => $"Talk with {name}";
    }
}
