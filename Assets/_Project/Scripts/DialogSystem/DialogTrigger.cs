using NodeCanvas.DialogueTrees;
using UnityEngine;

namespace DualityGame.DialogSystem
{
    [RequireComponent(typeof(DialogueTreeController))]
    public class DialogTrigger : MonoBehaviour
    {
        private DialogueTreeController _controller;
        protected void Awake() => _controller = GetComponent<DialogueTreeController>();
        private void OnTriggerEnter(Collider other)
        {
            var instigator = other.GetComponent<DialogueActor>();
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
