using DualityGame.Iteractables;
using Games.GrumpyBear.Core.SaveSystem;
using NodeCanvas.DialogueTrees;
using UnityEngine;

namespace DualityGame.Inventory
{
    [RequireComponent(typeof(SaveableEntity))]
    public class ItemPickup : Interactable, ISaveableComponent
    {
        [SerializeField] private ItemType _itemType;

        private DialogueTreeController _dialogueTreeController;
        private void Awake() => _dialogueTreeController = GetComponent<DialogueTreeController>();

        public override void Interact(GameObject actor)
        {
            var inventory = actor.GetComponent<InventoryController>();
            if (inventory == null) return;
            
            inventory.PickupItem(_itemType);
            gameObject.SetActive(false);

            if (_dialogueTreeController == null) return;
            var instigator = actor.GetComponent<DialogueActor>();
            if (instigator != null)
            {
                _dialogueTreeController.StartDialogue(instigator);
            }
            else
            {
                _dialogueTreeController.StartDialogue();
            }

        }
       
        public override bool Enabled => base.Enabled && gameObject.activeSelf;

        public override IInteractable.InteractionType Type => IInteractable.InteractionType.Touch;

        public override string ToString() => name;

        #if UNITY_EDITOR
        public void OnValidate()
        {
            if (_itemType == null) return;
            var spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.sprite = _itemType.InventorySprite;
        }
        #endif

        object ISaveableComponent.CaptureState() => gameObject.activeSelf;

        void ISaveableComponent.RestoreState(object state) => gameObject.SetActive((bool)state);
    }
}
