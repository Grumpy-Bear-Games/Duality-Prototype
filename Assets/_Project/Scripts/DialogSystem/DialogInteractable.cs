using DualityGame.Iteractables;
using NodeCanvas.DialogueTrees;
using UnityEngine;

namespace DualityGame.DialogSystem
{
    public class DialogInteractable : Interactable
    {
        [SerializeField] private IInteractable.InteractionType _type = IInteractable.InteractionType.Talk;

        private DialogueTreeController _controller;

        protected void Awake() => _controller = GetComponent<DialogueTreeController>();

        public override IInteractable.InteractionType Type => _type;

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
