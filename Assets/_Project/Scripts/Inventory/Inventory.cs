using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DualityGame.Inventory
{
    
    [CreateAssetMenu(fileName = "Inventory", menuName = "Duality/Inventory", order = 0)]
    public class Inventory : ScriptableObject
    {
        private readonly List<ItemType> _items = new();

        public IReadOnlyList<ItemType> Items => _items;

        public event Action OnChange;

        public void AddItem(ItemType item)
        {
            _items.Add(item);
            _SortItems();
            OnChange?.Invoke();
        }

        public int CountItemsOfType(ItemType itemType) => _items.Count(item => item == itemType);

        public ItemType RemoveItem(ItemType itemType)
        {
            try
            {
                var item = _items.First(item => item == itemType);
                _items.Remove(item);
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
    }
}
