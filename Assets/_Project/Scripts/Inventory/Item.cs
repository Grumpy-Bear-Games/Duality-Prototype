using System;
using DualityGame.SaveSystem;
using UnityEngine;

namespace DualityGame.Inventory
{
    [RequireComponent(typeof(SaveableEntity))]
    public class Item : MonoBehaviour, Iteractables.IInteractable, ISaveableComponent
    {
        [SerializeField] private ItemType _itemType;
        [SerializeField] private Vector3 _promptOffset;

        public ItemType Type => _itemType;

        public void Interact(GameObject actor)
        {
            var inventory = actor.GetComponent<InventoryController>();
            if (inventory == null) return;
            
            inventory.PickupItem(this);
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


        [Serializable]
        private class SerializableState
        {
            public readonly bool Active;

            public SerializableState(bool active)
            {
                Active = active;
            }
        }

        object ISaveableComponent.CaptureState()
        {
            var state = new SerializableState(gameObject.activeSelf);
            return state;
        }

        void ISaveableComponent.RestoreState(object state)
        {
            var serializableState = (SerializableState)state;
            gameObject.SetActive(serializableState.Active);
        }
    }
}
