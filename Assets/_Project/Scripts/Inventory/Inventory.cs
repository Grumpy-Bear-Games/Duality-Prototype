using System;
using System.Collections.Generic;
using System.Linq;
using Games.GrumpyBear.Core.SaveSystem;
using UnityEngine;

namespace DualityGame.Inventory
{
    
    [CreateAssetMenu(fileName = "Inventory", menuName = "Duality/Inventory", order = 0)]
    public class Inventory : ScriptableObject, ISaveableComponent
    {
        private readonly List<ItemType> _items = new();
        private ISaveableComponent saveableComponentImplementation;

        public IReadOnlyList<ItemType> Items => _items;

        public event Action OnChange;

        public void AddItem(ItemType item)
        {
            _items.Add(item);
            _SortItems();
            Notifications.Notifications.Add(item.InventorySprite, $"{item.name} added to inventory");
            OnChange?.Invoke();
        }

        public int CountItemsOfType(ItemType itemType) => _items.Count(item => item == itemType);

        public ItemType RemoveItem(ItemType itemType)
        {
            try
            {
                var item = _items.First(item => item == itemType);
                _items.Remove(item);
                Notifications.Notifications.Add(item.InventorySprite, $"{item.name} removed from inventory");
                OnChange?.Invoke();
                return item;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public void Clear()
        {
            _items.Clear();
            OnChange?.Invoke();
        }

        private void _SortItems() => _items.Sort((a, b) => string.Compare(a.name, b.name, StringComparison.Ordinal));

        #region ISaveableComponent
        object ISaveableComponent.CaptureState() => _items.Select(item => item.ObjectGuid).ToList();

        void ISaveableComponent.RestoreState(object state)
        {
            _items.Clear();
            foreach (var guid in (List<ObjectGuid>)state)
            {
                _items.Add(ItemType.GetByGuid(guid));
            }
            OnChange?.Invoke();
        }
        #endregion
    }
}
