using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.Playables;

namespace DualityGame.Playables.Tasks{

	[Category("Duality")]
	[Description("Trigger PlayableDirector")]
	public class TriggerPlayableDirector : DTNode
	{
		[RequiredField] public BBParameter<PlayableDirectorObservable> _playerableDirectorObservable;


		public override int maxOutConnections => 1;
		public override bool requireActorSelection => false;

		public override string name => _playerableDirectorObservable.isNoneOrNull ? $"Play playable" : $"Trigger {_playerableDirectorObservable.value.name}";

		protected override Status OnExecute(Component agent, IBlackboard bb)
		{
			var playableDirector = _playerableDirectorObservable.value.Value;
			playableDirector.Play();
			playableDirector.stopped += OnPlayableDirectorStopped;
			DLGTree.Pause();

			return Status.Running;
		}

		private void OnPlayableDirectorStopped(PlayableDirector obj)
		{
			DLGTree.Continue();
			DLGTree.Resume();
			var playableDirector = _playerableDirectorObservable.value.Value;
			playableDirector.stopped -= OnPlayableDirectorStopped;
		}
	}
}
