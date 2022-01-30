using DualityGame.Inventory;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace DualityGame.Quests{

	[Category("Duality")]
	[Description("Take the current item from an inventory")]
	public class InventoryTake : ActionTask
	{
		public BBParameter<Inventory.Inventory> _inventory;
		
		[BlackboardOnly]
		public BBParameter<Item> _saveAs;

		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit(){
			return null;
		}

		protected override string info => $"Take current item from {_inventory.value.name}";

		
		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute()
		{
			var item = _inventory.value.TakeFromInventory();
			if (_saveAs.isDefined) _saveAs.value = item; 
			EndAction(true);
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate(){
			
		}

		//Called when the task is disabled.
		protected override void OnStop(){
			
		}

		//Called when the task is paused.
		protected override void OnPause(){
			
		}
	}
}
