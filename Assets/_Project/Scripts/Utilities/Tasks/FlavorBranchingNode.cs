using System.Collections.Generic;
using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DualityGame.Utilities.Tasks
{
    [ParadoxNotion.Design.Icon("Condition")]
    [Name("Flavor Branching")]
    [Category("Branch")]
    [Description("Execute a new branch on every iteration")]
    [Color("b3ff7f")]
    public class FlavorBranchingNode : DTNode
    {
        #region Flavor Counters
        private static Dictionary<string, int> _flavorCounters = new();

        private static int IncrementFlavorCounter(string counterName)
        {
            _flavorCounters.TryAdd(counterName, 0);
            return _flavorCounters[counterName]++;
        }

        public static object CaptureState() => _flavorCounters;

        public static void RestoreState(object state)
        {
            if (state is not Dictionary<string, int> flavorBranchingCounters)
            {
                Debug.LogError("State is not a Dictionary<string, int>");
                Debug.Break();
                return;
            }
            _flavorCounters = flavorBranchingCounters;
        }
        #endregion

        [RequiredField] public BBParameter<string> _id;
        [RequiredField] public BBParameter<bool> _loop = false;

        public override int maxOutConnections => -1;

        public override bool requireActorSelection => false;

        public override string name => "Flavor Branching";

        public override void OnCreate(Graph assignedGraph)
        {
            _id.value = UID;
        }

        protected override Status OnExecute(Component agent, IBlackboard bb) {
            if ( outConnections.Count == 0 ) {
                return Error("There are no connections on the Flavor Branching Node");
            }
            var index = IncrementFlavorCounter(_id.value);
            if (index > outConnections.Count - 1)
                index = _loop.value ? index % outConnections.Count : outConnections.Count - 1;
            DLGTree.Continue(index);
            return Status.Success;
        }

        ///----------------------------------------------------------------------------------------------
        ///---------------------------------------UNITY EDITOR-------------------------------------------
        #if UNITY_EDITOR
        public override string GetConnectionInfo(int i)
        {
            if (_loop.value) return $"Every {CountSuffix(i + 1)} iteration";

            return i < outConnections.Count - 1 ? $"{CountSuffix(i + 1)} iteration" : $"{CountSuffix(i + 1)} iteration and beyond";
        }

        private string CountSuffix(int i)
        {
            return i switch
            {
                1 => $"{i}st",
                2 => $"{i}nd",
                3 => $"{i}rd",
                _ => $"{i}th"
            };
        }
#endif
    }
}
