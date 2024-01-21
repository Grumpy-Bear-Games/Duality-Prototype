using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace DualityGame.Inventory.Tasks{

	[Category("Duality")]
	[Description("Remove item from inventory")]
	public class RemoveItemFromInventory : ActionTask
	{
		[RequiredField] public BBParameter<Inventory> _inventory;
		[RequiredField] public BBParameter<ItemType> _itemType;

		protected override string info => _itemType.isNoneOrNull ? $"Remove item from inventory" : $"Remove {_itemType.value} from inventory";

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute()
		{
			var item = _inventory.value.RemoveItem(_itemType.value);
			EndAction(true);
		}

		public override void OnCreate(ITaskSystem ownerSystem) => _inventory.value = Resources.FindObjectsOfTypeAll<Inventory>().FirstOrDefault();
	}
}
