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

        public override void Interact()
        {
            var player = ServiceLocator.ServiceLocator.Get<Player.Player>();

            if (_dialogueTreeController == null)
            {
                PickupItem(player);
                return;
            }

            _dialogueTreeController.StartDialogue(player.Actor, success =>
            {
                if (success) PickupItem(player);
            });
        }

        private void PickupItem(Player.Player player)
        {
            player.InventoryController.PickupItem(_itemType);
            gameObject.SetActive(false);
        }
       
        public override IInteractable.InteractionType Type => IInteractable.InteractionType.Touch;

        public override string ToString() => name;

        #if UNITY_EDITOR
        public void OnValidate()
        {
            var spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.sprite = _itemType != null ? _itemType.InventorySprite : null;
        }
        #endif

        object ISaveableComponent.CaptureState() => gameObject.activeSelf;

        void ISaveableComponent.RestoreState(object state) => gameObject.SetActive((bool)state);
    }
}
