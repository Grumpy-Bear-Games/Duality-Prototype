using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace DualityGame.Quests.Tasks
{
    [Category("Duality")]
    [Description("Check state of quest")]
    public class CheckQuestState : ConditionTask
    {
        [RequiredField] public BBParameter<Quest> _quest;
        [RequiredField] public BBParameter<Quest.QuestStatus> _questStatus;

        protected override string info => (_quest.isNoneOrNull, _questStatus.isNoneOrNull) switch
        {
            (true, true) => "Quest has some state",
            (true, false) => $"Quest is {_questStatus.value}",
            (false, true) => $"Quest '{_quest.value.name}' has some state",
            (false, false) => $"Quest '{_quest.value.name}' is {_questStatus.value}",
        };

        //Called once per frame while the condition is active.
        //Return whether the condition is success or failure.
        protected override bool OnCheck()
        {
            if (_quest.isNoneOrNull) return Error("There is no Quest specified");
            if (_questStatus.isNoneOrNull) return Error("There is no Quest Status specified");

            return _quest.value.Status == _questStatus.value;
        }
    }
}
