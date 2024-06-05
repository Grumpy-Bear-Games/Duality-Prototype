using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace DualityGame.Quests.Tasks
{
    [Category("Duality")]
    [Description("Check if quest has started yet")]
    public class HasQuestStarted : ConditionTask
    {
        [RequiredField] public BBParameter<Quest> _quest;

        protected override string info => (_quest.isNoneOrNull) switch
        {
            true => "(Please specify quest)",
            false => $"{_quest.value.name} has started",
        };

        //Called once per frame while the condition is active.
        //Return whether the condition is success or failure.
        protected override bool OnCheck()
        {
            if (_quest.isNoneOrNull) return Error("There is no Quest specified");

            return _quest.value.Status != Quest.QuestStatus.NotStarted;
        }
    }
}
