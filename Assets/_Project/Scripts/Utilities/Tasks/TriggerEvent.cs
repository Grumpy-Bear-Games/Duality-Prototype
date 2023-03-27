using Games.GrumpyBear.Core.Events;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace DualityGame.Utilities.Tasks{

	[Category("Duality")]
	[Description("Trigger Event")]
	public class TriggerEvent : ActionTask
	{
		[RequiredField] public BBParameter<VoidEvent> _event;

		protected override string info => _event.isNoneOrNull ? $"Trigger event" : $"Trigger {_event.value.name}";

		protected override void OnExecute()
		{
			_event.value.Invoke();
			EndAction(true);
		}

	}
}
