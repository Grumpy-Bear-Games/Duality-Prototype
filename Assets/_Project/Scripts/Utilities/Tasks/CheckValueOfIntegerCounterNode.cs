using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace DualityGame.Utilities.Tasks
{

    [ParadoxNotion.Design.Icon("Condition")]
    [Name("Check counter")]
    [Category("Branch")]
    [Description("Execute branch depending on the value of a counter")]
    [Color("b3ff7f")]
    public class CheckValueOfIntegerCounterNode : DTNode
    {
        [RequiredField] public BBParameter<int> _counter;
        [RequiredField] public BBParameter<bool> _autoIncrease = false;

        public override int maxOutConnections => -1;

        public override bool requireActorSelection => false;

        public override string name => _counter.isNoneOrNull ? $"Check value of counter" : $"Check value of '{_counter.name}'";

        protected override Status OnExecute(Component agent, IBlackboard bb) {
            if ( outConnections.Count == 0 ) {
                return Error("There are no connections on the Dialogue Condition Node");
            }

            if (_counter.isNoneOrNull) return Error("There is no counter specified");

            var value = _counter.value; 
            DLGTree.Continue(Mathf.Min(value, outConnections.Count - 1));
            if (_autoIncrease.value) _counter.SetValue(_counter.value + 1);
            return Status.Success;
        }

        ///----------------------------------------------------------------------------------------------
        ///---------------------------------------UNITY EDITOR-------------------------------------------
        #if UNITY_EDITOR
        public override string GetConnectionInfo(int i)
        {
            var counterName = _counter.isNoneOrNull ? "counter" : _counter.name;
            return i < outConnections.Count - 1 ? $"{counterName} == {i}" : $"{counterName} >= {outConnections.Count - 1}";
        }
        #endif
    }
}
