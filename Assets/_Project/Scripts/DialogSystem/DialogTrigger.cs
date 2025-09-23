using NodeCanvas.DialogueTrees;
using UnityEngine;

namespace DualityGame.DialogSystem
{
    [RequireComponent(typeof(DialogueTreeController))]
    public class DialogTrigger : MonoBehaviour
    {
        private DialogueTreeController _controller;
        protected void Awake() => _controller = GetComponent<DialogueTreeController>();
        private void OnTriggerEnter(Collider _)
        {
            var player = ServiceLocator.ServiceLocator.Get<Player.Player>();
            _controller.StartDialogue(player.Actor);
        }
    }
}
