using DualityGame.Inventory;
using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace DualityGame.Quests{
	[Category("Duality")]
	[Description("Check if an inventory contains a specific item")]
	public class InventoryContainsItem : ConditionTask
	{
		[RequiredField]
		public BBParameter<Inventory.Inventory> _inventory;
		[RequiredField]
		public BBParameter<Item> _item;

		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit(){
			return null;
		}

		//Called whenever the condition gets enabled.
		protected override void OnEnable(){
			
		}

		//Called whenever the condition gets disabled.
		protected override void OnDisable(){
			
		}
		
		protected override string info => $"{_inventory.value.name} contains {_item.value.name}";

		//Called once per frame while the condition is active.
		//Return whether the condition is success or failure.
		protected override bool OnCheck(){
			return _inventory.value.CurrentItem.Value == _item.value;
		}
	}
}
