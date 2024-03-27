using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace DualityGame.Quests.Tasks
{
    [Category("Duality")]
    [Description("Start quest and add it to the quest log")]
    public class StartQuest: ActionTask
    {
        [RequiredField] public BBParameter<QuestLog> _questLog;
        [RequiredField] public BBParameter<Quest> _quest;

        protected override string info => _quest.isNoneOrNull ? "(Please specify quest)" : $"Start quest '{_quest.value.name}'";

        //This is called once each time the task is enabled.
        //Call EndAction() to mark the action as finished, either in success or failure.
        //EndAction can be called from anywhere.
        protected override void OnExecute()
        {
            _questLog.value.Add(_quest.value);
            EndAction(true);
        }

        public override void OnCreate(ITaskSystem ownerSystem) => _questLog.value = Resources.FindObjectsOfTypeAll<QuestLog>().FirstOrDefault();
    }
}
