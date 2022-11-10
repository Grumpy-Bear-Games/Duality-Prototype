using DualityGame.Inventory;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace DualityGame.Quests{

	[Category("Duality")]
	[Description("Check if an inventory contains a specific item")]
	public class InventoryContainsItemType : ConditionTask {
		
		[RequiredField] public BBParameter<Inventory.Inventory> _inventory;
		[RequiredField] public BBParameter<ItemType> _itemType;

		protected override string info => $"Inventory contains item of type {_itemType.name}";


		//Called once per frame while the condition is active.
		//Return whether the condition is success or failure.
		protected override bool OnCheck() => _inventory.value.CountItemsOfType(_itemType.value) > 0;
	}
}
