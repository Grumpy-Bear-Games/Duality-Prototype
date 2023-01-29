using DualityGame.Inventory;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace DualityGame.Quests{

	[Category("Duality")]
	[Description("Take the current item from an inventory")]
	public class InventoryTakeItemOfType : ActionTask
	{
		[RequiredField] public BBParameter<Inventory.Inventory> _inventory;
		[RequiredField] public BBParameter<ItemType> _itemType;

		protected override string info => $"Take item of type {_itemType.value} from inventory";
		
		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute()
		{
			var item = _inventory.value.RemoveItem(_itemType.value);
			EndAction(true);
		}

	}
}
