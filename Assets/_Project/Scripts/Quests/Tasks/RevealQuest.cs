using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace DualityGame.Quests.Tasks
{
    [Category("Duality")]
    [Description("Reveal quest")]
    public class RevealQuest : ActionTask
    {
        [RequiredField] public BBParameter<Quest> _quest;

        protected override string info => (_quest.isNoneOrNull) switch
        {
            true => "(Please specify quest)",
            false => $"Reveal quest '{_quest.value.name}'",
        };

        //This is called once each time the task is enabled.
        //Call EndAction() to mark the action as finished, either in success or failure.
        //EndAction can be called from anywhere.
        protected override void OnExecute()
        {
            if (_quest.isNoneOrNull)
            {
                Debug.LogError("There is no Quest specified");
                EndAction(false);
                return;
            }

            _quest.value.Reveal();
            EndAction(true);
        }
    }
}
