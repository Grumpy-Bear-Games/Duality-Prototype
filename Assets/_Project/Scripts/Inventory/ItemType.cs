using UnityEngine;

namespace DualityGame.Inventory
{
    [CreateAssetMenu(fileName = "Item Type", menuName = "Duality/Item Type", order = 0)]
    public class ItemType : ScriptableObject
    {
        [SerializeField] private Sprite _inventorySprite;
        [SerializeField] private Realm.Realm _realm;
        [SerializeField] private GameObject _prefab;

        public Sprite InventorySprite => _inventorySprite;

        public void Spawn(Realm.Realm realm, Vector3 position)
        {
            var go = Instantiate(_prefab, position, Quaternion.identity);
            go.layer = realm.LevelLayer;
            go.SetActive(true);
        }
    }
}
