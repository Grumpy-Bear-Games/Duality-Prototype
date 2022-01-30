using UnityEngine;

namespace DualityGame.Inventory
{
    public class Item : MonoBehaviour, Iteractables.IInteractable
    {
        [SerializeField] private ItemType _itemType;

        public ItemType Type => _itemType;

        public void Interact(GameObject actor)
        {
            var inventory = actor.GetComponent<Inventory>();
            if (inventory == null) return;
            
            inventory.PickupItem(this);
        }

        public string Prompt => $"Pick up {_itemType.name}";
        
        public override string ToString() => name;
    }
}
