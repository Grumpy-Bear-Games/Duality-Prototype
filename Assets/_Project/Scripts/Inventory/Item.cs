using UnityEngine;

namespace DualityGame.Inventory
{
    public class Item : MonoBehaviour, Iteractables.IInteractable
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
    }
}
