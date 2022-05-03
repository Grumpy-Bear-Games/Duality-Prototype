using DualityGame.Core;
using DualityGame.Iteractables;
using NodeCanvas.DialogueTrees;
using UnityEngine;

namespace DualityGame.Quests
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
        
        private void OnEnable()
        {
            DialogueTree.OnDialogueStarted += SetTalking;
            DialogueTree.OnDialogueFinished += SetMoving;
        }

        private void OnDisable()
        {
            DialogueTree.OnDialogueStarted -= SetTalking;
            DialogueTree.OnDialogueFinished -= SetMoving;
        }

        private void SetMoving(DialogueTree obj) => PlayState.Current.Value = PlayState.State.Moving;

        private void SetTalking(DialogueTree obj) => PlayState.Current.Value = PlayState.State.Talking;
    }
}
