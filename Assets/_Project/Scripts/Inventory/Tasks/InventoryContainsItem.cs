using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace DualityGame.Inventory.Tasks{

	[Category("Duality")]
	[Description("Check if an inventory contains a specific item")]
	public class InventoryContainsItem : ConditionTask {
		
		[RequiredField] public BBParameter<Inventory> _inventory;
		[RequiredField] public BBParameter<ItemType> _itemType;

		protected override string info => _itemType.isNoneOrNull ? $"player has item of type" : $"player has {_itemType.value.name}";


		//Called once per frame while the condition is active.
		//Return whether the condition is success or failure.
		protected override bool OnCheck() => _inventory.value.CountItemsOfType(_itemType.value) > 0;

		public override void OnCreate(ITaskSystem ownerSystem) => _inventory.value = Resources.FindObjectsOfTypeAll<Inventory>().FirstOrDefault();
	}
}
