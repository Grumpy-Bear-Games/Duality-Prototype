using System;
using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace DualityGame.Quests.Tasks
{
    [Category("Duality")]
    [Description("Resolve Quest")]
    public class ResolveQuest : ActionTask
    {
        [RequiredField] public BBParameter<QuestLog> _questLog;
        [RequiredField] public BBParameter<Quest> _quest;
        [RequiredField] public BBParameter<Resolution> _resolution;

        protected override string info => _quest.isNoneOrNull ? "(Please specify quest)" : $"Resolve quest as {_resolution.value}";

        //This is called once each time the task is enabled.
        //Call EndAction() to mark the action as finished, either in success or failure.
        //EndAction can be called from anywhere.
        protected override void OnExecute()
        {
            switch (_resolution.value)
            {
                case Resolution.Succeed:
                    _questLog.value.SucceedQuest(_quest.value);
                    break;
                case Resolution.Fail:
                    _questLog.value.FailQuest(_quest.value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            EndAction(true);
        }
        
        public enum Resolution
        {
            Succeed,
            Fail
        }

        public override void OnCreate(ITaskSystem ownerSystem) => _questLog.value = Resources.FindObjectsOfTypeAll<QuestLog>().FirstOrDefault();
    }
}
