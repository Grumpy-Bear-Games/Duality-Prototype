using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NodeCanvas.DialogueTrees
{
    [CreateAssetMenu(fileName = "Actor", menuName = "ParadoxNotion/NodeCanvas/Dialogue Trees/Actor Asset", order = 0)]
    public class ActorAsset : ScriptableObject
    {
        [Serializable]
        public struct MoodPortrait : IComparable<MoodPortrait>
        {
            public Mood Mood;
            public Sprite Sprite;

            int IComparable<MoodPortrait>.CompareTo(MoodPortrait other) => Mood.CompareTo(other.Mood);
        }

        [SerializeField] private string _name;
        [SerializeField] private List<MoodPortrait> _portraits = new();

        public string Name => _name;

        public Sprite PortraitByMood(Mood mood)
        {
            var sprite = _portraits.First(item => item.Mood == mood).Sprite;
            if (sprite == null) Debug.LogError($"Missing mood portrait for '{mood}'", this);
            return sprite;
        }

#if UNITY_EDITOR
        private void Reset()
        {
            _name = name;
            
            _portraits.Clear();
            foreach (Mood mood in Enum.GetValues(typeof(Mood)))
            {
                _portraits.Add(new MoodPortrait { Mood = mood } );
            }
        }

        private void OnValidate()
        {
            var values = new HashSet<Mood>(_portraits.Select(p => p.Mood));
            foreach (Mood mood in Enum.GetValues(typeof(Mood)))
            {
                if (values.Contains(mood)) continue;
                _portraits.Add(new MoodPortrait { Mood = mood } );
            }
            _portraits.Sort();
        }
#endif
        
        
    }
}
