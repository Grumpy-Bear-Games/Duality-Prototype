﻿using System;
using System.Collections.Generic;
using System.Linq;
using Games.GrumpyBear.Core.SaveSystem;
using Unity.Properties;
using UnityEngine;

namespace DualityGame.Quests
{
    [CreateAssetMenu(fileName = "Checkpoint", menuName = "Duality/Checkpoint", order = 0)]
    public class Checkpoint : SerializableScriptableObject<Checkpoint>
    {
        private static readonly HashSet<Checkpoint> _checkpoints = new();
        public static event Action AfterStateRestored;

        #if UNITY_EDITOR
        [CreateProperty]
        public string Name => name;
        #endif

        [CreateProperty]
        public bool Reached
        {
            get => _checkpoints.Contains(this);
            set
            {
                if (value)
                {
                    _checkpoints.Add(this);
                }
                else
                {
                    _checkpoints.Remove(this);
                }
            }
        }

        public static object CaptureState() => _checkpoints.Select(checkpoint => checkpoint.ObjectGuid).ToList();

        public static void RestoreState(object state)
        {
            if (state is not List<ObjectGuid> guids)
            {
                Debug.LogError("Expected state to be a list of ObjectGuids");
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
