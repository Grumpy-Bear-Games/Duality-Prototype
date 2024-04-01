using Games.GrumpyBear.Core.SaveSystem;
using Unity.Properties;
using UnityEngine;

namespace DualityGame.Inventory
{
    [CreateAssetMenu(fileName = "Item Type", menuName = "Duality/Item Type", order = 0)]
    public class ItemType : SerializableScriptableObject<ItemType>
    {
        [SerializeField] private Sprite _inventorySprite;
        [SerializeField] private Realm.Realm _realm;

        #if UNITY_EDITOR
        [CreateProperty] public string Name => name;
        #endif

        [CreateProperty] public Sprite InventorySprite => _inventorySprite;
        [CreateProperty] public Realm.Realm Realm => _realm;

        public override string ToString() => name;
    }
}
