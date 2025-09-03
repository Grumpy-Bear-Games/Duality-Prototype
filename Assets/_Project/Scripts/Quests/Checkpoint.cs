using System;
using System.Collections.Generic;
using System.Linq;
using Games.GrumpyBear.Core.SaveSystem;
using Unity.Properties;
using UnityEngine;

namespace DualityGame.Quests
{
    [CreateAssetMenu(fileName = "Checkpoint", menuName = "Duality/Checkpoint", order = 0), GeneratePropertyBag]
    public class Checkpoint : SerializableScriptableObject<Checkpoint>
    {
        private static readonly HashSet<Checkpoint> _checkpoints = new();

        #region Static events
        public static event Action AfterStateRestored;
        public static event Action<Checkpoint> OnCheckpointUpdated;
        #endregion

        [SerializeField] private string _questLogEntry;
        [SerializeField] private string _questLogEntryWhenReached;

        #region Properties
        [CreateProperty]
        public string Name => name;

        [CreateProperty]
        public string QuestLogEntry =>
            (Reached, !string.IsNullOrEmpty(_questLogEntry), !string.IsNullOrEmpty(_questLogEntryWhenReached)) switch
                {
                    (false, false, _) or (true, false, false) => Name,
                    (false, true, _) or (true, true, false) => _questLogEntry,
                    (true, _, true) => _questLogEntryWhenReached
                };

        [CreateProperty]
        public bool Reached
        {
            get => _checkpoints.Contains(this);
            set
            {
                if (value == Reached) return;
                if (value)
                {
                    _checkpoints.Add(this);
                }
                else
                {
                    _checkpoints.Remove(this);
                }
                OnCheckpointUpdated?.Invoke(this);
            }
        }
        #endregion

        public static object CaptureState() => _checkpoints.Select(checkpoint => checkpoint.ObjectGuid).ToList();

        public static void RestoreState(object state)
        {
            if (state is not List<ObjectGuid> guids)
            {
                Debug.LogError($"Unexpected object type {state.GetType()}");
                return;
            }

            _checkpoints.Clear();
            foreach (var checkpoint in guids.Select(GetByGuid).Where(checkpoint => checkpoint != null))
            {
                _checkpoints.Add(checkpoint);
            }
            AfterStateRestored?.Invoke();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void ClearAll()
        {
            _checkpoints.Clear();
            AfterStateRestored?.Invoke();
        }
    }
}
