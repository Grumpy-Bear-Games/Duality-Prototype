using System.Collections.Generic;
using Games.GrumpyBear.Core.LevelManagement;
using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace DualityGame.Utilities.Tasks
{
    [ParadoxNotion.Design.Icon("Condition")]
    [Name("Scene Group Branching")]
    [Category("Branch")]
    [Description("Branch based on the current scene group")]
    [Color("b3ff7f")]
    public class SceneGroupBranching : DTNode
    {
        [SerializeField]
        public List<SceneGroup> _sceneGroups = new();

        public override int maxOutConnections => _sceneGroups.Count + 1;
        public override bool requireActorSelection => false;

        public override string name => "Scene Group Branching";

        protected override Status OnExecute(Component agent, IBlackboard bb) {
            if ( outConnections.Count == 0 ) {
                return Error("There are no connections on the SceneGroupBranching Node");
            }

            for (var i = 0; i < _sceneGroups.Count; i++)
            {
                var sceneGroup = _sceneGroups[i];

                if (sceneGroup ==null) return Error("There is a missing SceneGroup");
                if (SceneManager.CurrentSceneGroup.Value != sceneGroup) continue;
                DLGTree.Continue(i);
                return Status.Success;
            }
            DLGTree.Continue(_sceneGroups.Count);
            return Status.Success;
        }

        ///----------------------------------------------------------------------------------------------
        ///---------------------------------------UNITY EDITOR-------------------------------------------
#if UNITY_EDITOR
        public override string GetConnectionInfo(int i)
        {
            if (i >= _sceneGroups.Count) return "<i>Else</i>";
            return _sceneGroups[i] == null ? ErrorText("Missing SceneGroup") : $"When in {_sceneGroups[i].name}";
        }

        private string ErrorText(string text) => $"<color=red><i><b>{text}</b></i></color>";
#endif
    }
}
