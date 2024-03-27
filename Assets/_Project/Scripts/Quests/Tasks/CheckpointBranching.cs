using System.Collections.Generic;
using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.Serialization;

namespace DualityGame.Quests.Tasks
{
    [ParadoxNotion.Design.Icon("Condition")]
    [Name("Checkpoint Branching")]
    [Category("Branch")]
    [Description("Branch based on which checkpoint has been reached")]
    [Color("b3ff7f")]
    public class CheckpointBranching : DTNode
    {
        [System.Serializable]
        public class CheckpointEntry
        {
            public bool isUnfolded = true;
            public Checkpoint Checkpoint;
            public bool AutoReach = false;
        }

        [SerializeField]
        private List<CheckpointEntry> _checkpointEntries = new();

        public override int maxOutConnections => _checkpointEntries.Count + 1;
        public override bool requireActorSelection => false;

        public override string name => "Checkpoint Branching";

        protected override Status OnExecute(Component agent, IBlackboard bb) {
            if ( outConnections.Count == 0 ) {
                return Error("There are no connections on the Dialogue Condition Node");
            }

            for (var i = 0; i < _checkpointEntries.Count; i++)
            {
                var entry = _checkpointEntries[i];
                if (entry.Checkpoint == null) return Error("There is a missing checkpoint");
                if (entry.Checkpoint.Reached) continue;
                if (entry.AutoReach) _checkpointEntries[i].Checkpoint.Reached = true;
                DLGTree.Continue(i);
                return Status.Success;
            }
            DLGTree.Continue(_checkpointEntries.Count);
            return Status.Success;
        }

        ///----------------------------------------------------------------------------------------------
        ///---------------------------------------UNITY EDITOR-------------------------------------------
#if UNITY_EDITOR
        public override void OnConnectionInspectorGUI(int i)
        {
            switch (i)
            {
                case 0:
                    GUILayout.Label("<i>No checkpoint reached yet</i>");
                    break;
                case > 0:
                    DoCheckpointEntryUI(_checkpointEntries[i-1]);
                    break;
            }
        }

        protected override void OnNodeGUI() {

            if ( _checkpointEntries.Count == 0 ) {
                GUILayout.Label("<i>No checkpoints defined</i>");
                return;
            }

            foreach (var entry in _checkpointEntries)
            {
                GUILayout.BeginHorizontal(Styles.roundedBox);
                GUILayout.Label(
                    $"{(entry.AutoReach ? "■" : "□")} {(entry.Checkpoint != null ? entry.Checkpoint.name.CapLength(30) : ErrorText("Missing checkpoint"))}", Styles.leftLabel);
                GUILayout.EndHorizontal();
            }
        }


        protected override void OnNodeInspectorGUI() {

            base.OnNodeInspectorGUI();

            if ( GUILayout.Button("Add Checkpoint") )
            {
                _checkpointEntries.Add(new CheckpointEntry());
            }

            if ( _checkpointEntries.Count == 0 ) {
                return;
            }

            EditorUtils.ReorderableList(_checkpointEntries, (i, picked) =>
            {
                var entry = _checkpointEntries[i];
                GUILayout.BeginHorizontal("box");

                var text = string.Format("{0} {1}", entry.isUnfolded ? "▼ " : "► ", entry.Checkpoint != null ? entry.Checkpoint.name : ErrorText("Missing checkpoint"));
                if ( GUILayout.Button(text, (GUIStyle)"label", GUILayout.Width(0), GUILayout.ExpandWidth(true)) ) {
                    entry.isUnfolded = !entry.isUnfolded;
                }

                if ( GUILayout.Button("X", GUILayout.Width(20)) ) {
                    _checkpointEntries.RemoveAt(i);
                    if ( i < outConnections.Count ) {
                        graph.RemoveConnection(outConnections[i+1]);
                    }
                }

                GUILayout.EndHorizontal();

                if ( entry.isUnfolded ) {
                    DoCheckpointEntryUI(entry);
                }
            });
        }

        private void DoCheckpointEntryUI(CheckpointEntry entry) {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.BeginVertical("box");

            entry.Checkpoint = UnityEditor.EditorGUILayout.ObjectField("Checkpoint", entry.Checkpoint, typeof(Checkpoint), false) as Checkpoint;
            entry.AutoReach = UnityEditor.EditorGUILayout.Toggle("Automatically reached", entry.AutoReach);

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
        }

        public override string GetConnectionInfo(int i)
        {
            if (i == 0) return "No checkpoint reached yet";
            if (i-1 >= _checkpointEntries.Count) return "<i>No checkpoint defined for this connection</i>";
            return _checkpointEntries[i-1].Checkpoint == null ? ErrorText("Missing checkpoint") : $"'{_checkpointEntries[i-1].Checkpoint.name}' reached";
        }

        private string ErrorText(string text) => $"<color=red><i><b>{text}</b></i></color>";
#endif
    }
}
