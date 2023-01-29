using DualityGame.SaveSystem;
using UnityEngine;

namespace DualityGame.Inventory
{
    [RequireComponent(typeof(SaveableEntity))]
    public class ItemPickup : MonoBehaviour, Iteractables.IInteractable, ISaveableComponent
    {
        [SerializeField] private ItemType _itemType;
        [SerializeField] private Vector3 _promptOffset;

        public void Interact(GameObject actor)
        {
            var inventory = actor.GetComponent<InventoryController>();
            if (inventory == null) return;
            
            inventory.PickupItem(_itemType);
            gameObject.SetActive(false);
        }
       
        public Vector3 PromptPosition => transform.position + _promptOffset;

        public string Prompt => $"Pick up {_itemType.name}";
        
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
