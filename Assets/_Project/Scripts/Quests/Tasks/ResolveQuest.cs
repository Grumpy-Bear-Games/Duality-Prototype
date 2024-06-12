using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace DualityGame.Quests.Tasks
{
    [Category("Duality")]
    [Description("Resolve Quest")]
    public class ResolveQuest : ActionTask
    {
        [RequiredField] public BBParameter<Quest> _quest;
        [RequiredField] public BBParameter<Resolution> _resolution;

        protected override string info => (_quest.isNoneOrNull, _resolution.isNoneOrNull) switch
        {
            (true, true) => "(Please specify quest and resolution)",
            (true, false) => "(Please specify quest)",
            (false, true) => "(Please specify resolution)",
            (false, false) => $"Resolve quest '{_quest.value.name}' with {_resolution.value}",
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

            if (_resolution.isNoneOrNull)
            {
                Debug.LogError("There is no Resolution specified");
                EndAction(false);
                return;
            }

            switch (_resolution.value)
            {
                case Resolution.Succeed:
                    _quest.value.Succeed();
                    break;
                case Resolution.Fail:
                    _quest.value.Fail();
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
