using Games.GrumpyBear.Core.LevelManagement;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace DualityGame.Utilities.Tasks{

	[Category("Duality")]
	[Description("Check if we are in specific scene group")]
	public class CheckCurrentSceneGroup : ConditionTask {
		
		[RequiredField] public BBParameter<SceneGroup> _currentSceneGroup;

		protected override string info => _currentSceneGroup.isNoneOrNull ? $"Current SceneGroup is <unset>" : $"Current SceneGroup is {_currentSceneGroup.value.name}";


		//Called once per frame while the condition is active.
		//Return whether the condition is success or failure.
		protected override bool OnCheck()
		{
			if (_currentSceneGroup.isNoneOrNull) return false;
			return SceneManager.CurrentSceneGroup.Value == _currentSceneGroup.value;
		}
	}
}
