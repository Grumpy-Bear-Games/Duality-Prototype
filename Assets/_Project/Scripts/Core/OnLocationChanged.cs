using Games.GrumpyBear.LevelManagement;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace DualityGame.Core{
	[Category("Duality")]
	[Description("When location changes")]
	public class OnLocationChanged : ConditionTask
	{
		private bool _locationChanged;
		
		protected override string OnInit(){
			return null;
		}

		//Called whenever the condition gets enabled.
		protected override void OnEnable() => LocationManager.OnLocationChanged += LocationChanged;

		private void LocationChanged(Location location) => _locationChanged = true;

		//Called whenever the condition gets disabled.
		protected override void OnDisable() => LocationManager.OnLocationChanged -= LocationChanged;

		//Called once per frame while the condition is active.
		//Return whether the condition is success or failure.
		protected override bool OnCheck()
		{
			var locationChanged = _locationChanged;
			_locationChanged = false;
			return locationChanged;
		}
	}
}
