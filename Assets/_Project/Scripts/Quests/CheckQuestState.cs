using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace DualityGame.Quests
{
    [Category("Duality")]
    [Description("Check state of quest")]
    public class CheckQuestState : ConditionTask
    {
        [RequiredField] public BBParameter<Quest> _quest;
        [RequiredField] public BBParameter<Quest.QuestState> _questState;

        protected override string info => $"Quest is {_questState.value}";

        //Called once per frame while the condition is active.
        //Return whether the condition is success or failure.
        protected override bool OnCheck() => !_quest.isNull && _quest.value.State == _questState.value;
    }
}