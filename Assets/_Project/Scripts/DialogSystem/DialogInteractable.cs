using DualityGame.Iteractables;
using NodeCanvas.DialogueTrees;
using UnityEngine;

namespace DualityGame.DialogSystem
{
    public class DialogInteractable : Interactable
    {
        private DialogueTreeController _controller;

        protected void Awake() => _controller = GetComponent<DialogueTreeController>();

        public override IInteractable.InteractionType Type => IInteractable.InteractionType.Talk;

        public override void Interact(GameObject actor)
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
    }
}
