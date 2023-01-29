using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace DualityGame.Quests.Tasks
{
    [Category("Duality")]
    [Description("Reveal quest")]
    public class RevealQuest : ActionTask
    {
        [RequiredField] public BBParameter<QuestLog> _questLog;
        [RequiredField] public BBParameter<Quest> _quest;

        protected override string info => _quest.isNoneOrNull ? "(Please specify quest)" : "Reveal quest";

        //This is called once each time the task is enabled.
        //Call EndAction() to mark the action as finished, either in success or failure.
        //EndAction can be called from anywhere.
        protected override void OnExecute()
        {
            if (_quest.isNull || _questLog.isNull)
            {
                // TODO: Warnings
                EndAction(true);
                return;
            }
            
            var entry = _questLog.value.GetEntry(_quest.value);
            // TODO: Null check
            
            entry.Visible = true;
            EndAction(true);
        }
    }
}
