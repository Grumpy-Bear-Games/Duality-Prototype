using System;
using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace DualityGame.Quests.Tasks
{

    [ParadoxNotion.Design.Icon("Condition")]
    [Name("Quest State")]
    [Category("Branch")]
    [Description("Execute one of up to four child nodes depending on the state of the quest")]
    [Color("b3ff7f")]
    public class QuestStateNode : DTNode
    {
        [RequiredField] public BBParameter<Quest> _quest;

        public override int maxOutConnections => 3;
        public override bool requireActorSelection => false;

        public override string name => (_quest.isNoneOrNull) switch
        {
            true => "(Please specify quest)",
            false =>$"Check state of quest '{_quest.value.name}'",
        };

        protected override Status OnExecute(Component agent, IBlackboard bb) {
            if ( outConnections.Count == 0 ) {
                return Error("There are no connections on the Dialogue Condition Node");
            }

            if (_quest.isNoneOrNull) return Error("There is no Quest specified");

            switch (_quest.value.Status)
            {
                case Quest.QuestStatus.NotStarted:
                    DLGTree.Continue(0);
                    return Status.Failure;
                case Quest.QuestStatus.Ongoing:
                    DLGTree.Continue(1);
                    break;
                case Quest.QuestStatus.Completed:
                    DLGTree.Continue(2);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return Status.Success;
        }

        ///----------------------------------------------------------------------------------------------
        ///---------------------------------------UNITY EDITOR-------------------------------------------
        #if UNITY_EDITOR
        public override string GetConnectionInfo(int i)
        {
            return i switch
            {
                0 => "Not started",
                1 => "Ongoing",
                2 => "Complete",
                _ => throw new ArgumentOutOfRangeException(nameof(i), i, null)
            };
        }
        #endif
    }
}
