using DualityGame.Inventory;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace DualityGame.Quests{

	[Category("Duality")]
	[Description("Take the current item from an inventory")]
	public class InventoryTake : ActionTask
	{
		[RequiredField] public BBParameter<Inventory.Inventory> _inventory;
		[BlackboardOnly] public BBParameter<Item> _saveAs;

		protected override string info => "Take current item from inventory";
		
		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute()
		{
			var item = _inventory.value.TakeItem();
			if (_saveAs.isDefined) _saveAs.value = item; 
			EndAction(true);
		}

	}
}
