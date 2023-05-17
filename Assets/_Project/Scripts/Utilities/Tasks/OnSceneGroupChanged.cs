using Games.GrumpyBear.Core.LevelManagement;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace DualityGame.Utilities.Tasks{
	[Category("Duality")]
	[Description("When scene group changes")]
	public class OnSceneGroupChanged : ConditionTask
	{
		private bool _sceneGroupChanged;
		
		protected override string OnInit(){
			return null;
		}

		//Called whenever the condition gets enabled.
		protected override void OnEnable() => SceneManager.CurrentSceneGroup.OnChange += SceneGroupChanged;

		private void SceneGroupChanged(SceneGroup sceneGroup) => _sceneGroupChanged = true;

		//Called whenever the condition gets disabled.
		protected override void OnDisable() => SceneManager.CurrentSceneGroup.OnChange -= SceneGroupChanged;

		//Called once per frame while the condition is active.
		//Return whether the condition is success or failure.
		protected override bool OnCheck()
		{
			var sceneGroupChanged = _sceneGroupChanged;
			_sceneGroupChanged = false;
			return sceneGroupChanged;
		}
	}
}
