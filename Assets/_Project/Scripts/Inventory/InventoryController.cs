using Games.GrumpyBear.Core.SaveSystem;
using UnityEngine;

namespace DualityGame.Inventory
{
    public class InventoryController : MonoBehaviour, ISaveableComponent
    {
        [SerializeField] private Inventory _inventory;

        public void PickupItem(ItemType item) => _inventory.AddItem(item);

        #region ISaveableComponent
        object ISaveableComponent.CaptureState() => ((ISaveableComponent)_inventory).CaptureState();
        void ISaveableComponent.RestoreState(object state) => ((ISaveableComponent)_inventory).RestoreState(state);
        #endregion
    }
}
