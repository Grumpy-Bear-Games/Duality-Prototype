using DualityGame.Inventory;
using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace DualityGame.Quests{
	[Category("Duality")]
	[Description("Check if an inventory contains a specific item")]
	public class InventoryContainsItem : ConditionTask
	{
		[RequiredField] public BBParameter<Inventory.Inventory> _inventory;
		[RequiredField] public BBParameter<Item> _item;

		protected override string info => $"Inventory contains {_item.name}";

		//Called once per frame while the condition is active.
		//Return whether the condition is success or failure.
		protected override bool OnCheck() => _inventory.value.ContainsItem(_item.value);
	}
}
