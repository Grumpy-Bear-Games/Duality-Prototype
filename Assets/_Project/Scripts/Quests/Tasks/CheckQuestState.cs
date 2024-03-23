using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace DualityGame.Quests.Tasks
{
    [Category("Duality")]
    [Description("Check state of quest")]
    public class CheckQuestState : ConditionTask
    {
        [RequiredField] public BBParameter<QuestLog> _questLog;
        [RequiredField] public BBParameter<Quest> _quest;
        [RequiredField] public BBParameter<QuestLog.QuestState> _questState;

        protected override string info => (_quest.isNoneOrNull, _questState.isNoneOrNull) switch
        {
            (true, true) => "Quest has some state",
            (true, false) => $"Quest is {_questState.value}",
            (false, true) => $"Quest '{_quest.value.name}' has some state",
            (false, false) => $"Quest '{_quest.value.name}' is {_questState.value}",
        };

        //Called once per frame while the condition is active.
        //Return whether the condition is success or failure.
        protected override bool OnCheck()
        {
            var entry = _questLog.value.GetEntry(_quest.value);
            return entry != null && entry.State == _questState.value;
        }

        public override void OnCreate(ITaskSystem ownerSystem) =>
            _questLog.value = Resources.FindObjectsOfTypeAll<QuestLog>().FirstOrDefault();
    }
}
