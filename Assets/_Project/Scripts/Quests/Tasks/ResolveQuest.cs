using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace DualityGame.Quests.Tasks
{
    [Category("Duality")]
    [Description("Resolve Quest")]
    public class ResolveQuest : ActionTask
    {
        [RequiredField] public BBParameter<QuestLog> _questLog;
        [RequiredField] public BBParameter<Quest> _quest;
        [RequiredField] public BBParameter<Resolution> _resolution;

        protected override string info => _quest.isNoneOrNull ? "(Please specify quest)" : $"Set quest state to {_resolution.value}";

        //This is called once each time the task is enabled.
        //Call EndAction() to mark the action as finished, either in success or failure.
        //EndAction can be called from anywhere.
        protected override void OnExecute()
        {
            if (_quest.isNull || _questLog.isNull || _resolution.isNull)
            {
                // TODO: Warnings
                EndAction(true);
                return;
            }

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
    }
}
