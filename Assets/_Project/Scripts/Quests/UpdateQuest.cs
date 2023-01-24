using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace DualityGame.Quests
{
    [Category("Duality")]
    [Description("Update quest")]
    public class UpdateQuest : ActionTask
    {
        [RequiredField] public BBParameter<Quest> _quest;
        [RequiredField] public BBParameter<Quest.QuestState> _questState;

        protected override string info => _quest.isNoneOrNull ? "(Please specify quest)" : $"Set quest state to {_questState.value}";

        //This is called once each time the task is enabled.
        //Call EndAction() to mark the action as finished, either in success or failure.
        //EndAction can be called from anywhere.
        protected override void OnExecute()
        {
            _quest.value.State = _questState.value;
            EndAction(true);
        }
    }
}
