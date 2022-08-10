using Games.GrumpyBear.Core.Observables.ScriptableObjects;
using UnityEngine;

namespace DualityGame.Inventory
{
    [CreateAssetMenu(fileName = "Inventory", menuName = "Duality/Inventory", order = 0)]
    public class Inventory : Observable<Item>
    {
        public Item TakeItem()
        {
            if (Value == null) return null;
            var item = Value;
            Set(null);
            return item;
        }

        public bool ContainsItem(Item item) => Value == item;
        public bool ContainsItemOfType(ItemType itemType) => Value != null && Value.Type == itemType;
    }
}
