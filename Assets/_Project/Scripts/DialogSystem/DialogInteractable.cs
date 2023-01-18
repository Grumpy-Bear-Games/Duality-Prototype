using DualityGame.Iteractables;
using NodeCanvas.DialogueTrees;
using UnityEngine;

namespace DualityGame.DialogSystem
{
    public class DialogInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private Vector3 _promptOffset;
        
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

        public Vector3 PromptPosition => transform.position + _promptOffset;

        public string Prompt => $"Talk with {name}";
    }
}
