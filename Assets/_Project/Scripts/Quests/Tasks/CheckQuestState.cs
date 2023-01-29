using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace DualityGame.Quests.Tasks
{
    [Category("Duality")]
    [Description("Check state of quest")]
    public class CheckQuestState : ConditionTask
    {
        [RequiredField] public BBParameter<QuestLog> _questLog;
        [RequiredField] public BBParameter<Quest> _quest;
        [RequiredField] public BBParameter<QuestLog.QuestState> _questState;

        protected override string info => $"Quest state is {_questState.value}";

        //Called once per frame while the condition is active.
        //Return whether the condition is success or failure.
        protected override bool OnCheck()
        {
            var entry = _questLog.value.GetEntry(_quest.value);
            return entry != null && entry.State == _questState.value;
        }
    }
}
