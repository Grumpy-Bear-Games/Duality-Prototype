using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace DualityGame.Player.Tasks{

	[Name("Jump To Spawnpoint")]
	[Category("Duality")]
	[Description("Jump to Spawnpoint")]
	public class JumpToSpawnpointNode : DTNode
	{
		[RequiredField] public BBParameter<SpawnPointReference> _spawnPointReference;
		[RequiredField] public BBParameter<SpawnSettings> _spawnSettings;

		public override int maxOutConnections => 0;
		public override bool requireActorSelection => false;

		public override string name => "Jump to " + (_spawnPointReference.isNoneOrNull ? "spawnpoint" : $"\"{_spawnPointReference.value.name}\"");

		protected override Status OnExecute(Component agent, IBlackboard bb)
		{
			if (_spawnPointReference.isNoneOrNull)
			{
				Debug.LogError("Spawnpoint is null");
				return Status.Failure;
			}
			if (_spawnSettings.isNoneOrNull)
			{
				Debug.LogError("SpawnSettings is null");
				return Status.Failure;
			}
			_spawnPointReference.value.SpawnAt(_spawnSettings.value);
			//DLGTree.Stop();

			return Status.Success;
		}
	}
}
