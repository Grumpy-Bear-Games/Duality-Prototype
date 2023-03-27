using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace DualityGame.Inventory.Tasks{

	[Category("Duality")]
	[Description("Add item to inventory")]
	public class AddItemToInventory : ActionTask
	{
		[RequiredField] public BBParameter<Inventory> _inventory;
		[RequiredField] public BBParameter<ItemType> _itemType;

		protected override string info => _itemType.isNoneOrNull ? $"Add item to inventory" : $"Add {_itemType.value} to inventory";

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute()
		{
			_inventory.value.AddItem(_itemType.value);
			EndAction(true);
		}

	}
}
