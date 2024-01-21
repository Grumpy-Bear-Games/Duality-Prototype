using System;
using System.Linq;
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
        [RequiredField] public BBParameter<QuestLog> _questLog;
        [RequiredField] public BBParameter<Quest> _quest;

        public override int maxOutConnections => 4;
        public override bool requireActorSelection => false;

        public override string name => _quest.isNoneOrNull ? $"Check state of quest" : $"Check state of quest '{_quest.value.name}'";

        protected override Status OnExecute(Component agent, IBlackboard bb) {
            if ( outConnections.Count == 0 ) {
                return Error("There are no connections on the Dialogue Condition Node");
            }

            if (_questLog.isNoneOrNull) return Error("There is no Quest Log specified");
            if (_quest.isNoneOrNull) return Error("There is no Quest specified");

            var questLog = _questLog.value;
            var quest = _quest.value;


            if (!questLog.Contains(quest))
            {
                DLGTree.Continue(0);
                return Status.Failure;
            }

            var entry = questLog.GetEntry(quest);
            switch (entry.State)
            {
                case QuestLog.QuestState.Ongoing:
                    DLGTree.Continue(1);
                    break;
                case QuestLog.QuestState.Succeeded:
                    DLGTree.Continue(2);
                    break;
                case QuestLog.QuestState.Failed:
                    DLGTree.Continue(outConnections.Count < 4 ? 2 : 3);
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
                2 => outConnections.Count < 4 ? "Completed (success or failure)" : "Succeeded",
                _ => "Failed"
            };
        }
        #endif

        public override void OnCreate(Graph assignedGraph) => _questLog.value = Resources.FindObjectsOfTypeAll<QuestLog>().FirstOrDefault();
    }
}
