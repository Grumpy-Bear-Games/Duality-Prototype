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

        public override void Interact()
        {
            var player = ServiceLocator.ServiceLocator.Get<Player.Player>();
            _controller.StartDialogue(player.Actor);
        }
    }
}
