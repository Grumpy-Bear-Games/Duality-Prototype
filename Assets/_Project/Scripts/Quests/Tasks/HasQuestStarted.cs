using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace DualityGame.Quests.Tasks
{
    [Category("Duality")]
    [Description("Check if quest has started yet")]
    public class HasQuestStarted : ConditionTask
    {
        [RequiredField] public BBParameter<QuestLog> _questLog;
        [RequiredField] public BBParameter<Quest> _quest;

        protected override string info => $"Quest has started";

        //Called once per frame while the condition is active.
        //Return whether the condition is success or failure.
        protected override bool OnCheck()
        {
            if (_quest.isNull || _questLog.isNull) return false;
            // TODO: Warnings

            return _questLog.value.Contains(_quest.value);
        }
    }
}
