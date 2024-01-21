using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace DualityGame.Quests.Tasks
{
    [Name("Set checkpoint reached")]
    [Category("Duality")]
    [Description("Mark checkpoint as reached")]
    public class SetCheckpointReached : ActionTask
    {
        [RequiredField] public BBParameter<Checkpoint> _checkpoint;

        protected override string info => _checkpoint.isNoneOrNull ? "(Please specify checkpoint)" : $"Checkpoint \"{_checkpoint.value.name}\" reached";

        protected override void OnExecute()
        {
            _checkpoint.value.Reached = true;
            EndAction(true);
        }
    }
}
