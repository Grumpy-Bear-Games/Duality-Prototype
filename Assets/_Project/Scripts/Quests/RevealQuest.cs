using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace DualityGame.Quests
{
    [Category("Duality")]
    [Description("Reveal quest")]
    public class RevealQuest : ActionTask
    {
        [RequiredField] public BBParameter<Quest> _quest;

        protected override string info => _quest.isNoneOrNull ? "(Please specify quest)" : "Reveal quest";

        //This is called once each time the task is enabled.
        //Call EndAction() to mark the action as finished, either in success or failure.
        //EndAction can be called from anywhere.
        protected override void OnExecute()
        {
            _quest.value.Reveal();
            EndAction(true);
        }
    }
}