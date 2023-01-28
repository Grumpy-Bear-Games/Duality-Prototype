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

        private Vector3 _initialPosition;
        private int _initialLayer;

        private void Awake() => UpdateInitialPosition();

        public void ReturnToInitialPosition()
        {
            transform.position = _initialPosition;
            gameObject.layer = _initialLayer;
            gameObject.SetActive(true);
        }

        public void UpdateInitialPosition()
        {
            _initialPosition = transform.position;
            _initialLayer = gameObject.layer;
        }

        public void Interact(GameObject actor)
        {
            var inventory = actor.GetComponent<InventoryController>();
            if (inventory == null) return;
            
            inventory.PickupItem(this);
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
            public readonly Vector3 InitialPosition;
            public readonly bool Active;

            public SerializableState(Vector3 initialPosition, bool active)
            {
                InitialPosition = initialPosition;
                Active = active;
            }
        }

        object ISaveableComponent.CaptureState()
        {
            var state = new SerializableState(_initialPosition, gameObject.activeSelf);
            return state;
        }

        void ISaveableComponent.RestoreState(object state)
        {
            var serializableState = (SerializableState)state;
            _initialPosition = serializableState.InitialPosition;
            gameObject.SetActive(serializableState.Active);
        }
    }
}
