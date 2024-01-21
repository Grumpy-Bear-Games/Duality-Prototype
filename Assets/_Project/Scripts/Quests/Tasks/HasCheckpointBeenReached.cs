using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace DualityGame.Quests.Tasks
{
    [Name("Has checkpoint been reached?")]
    [Category("Duality")]
    [Description("Check if checkpoint has been reached")]
    public class HasCheckpointBeenReached : ConditionTask
    {
        [RequiredField] public BBParameter<Checkpoint> _checkpoint;

        protected override string info => _checkpoint.isNoneOrNull ? $"Checkpoint reached" : $"Checkpoint \"{_checkpoint.value.name}\" reached";

        protected override bool OnCheck() => _checkpoint.value.Reached;
    }
}
