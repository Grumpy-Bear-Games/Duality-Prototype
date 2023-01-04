using System;
using System.Collections.Generic;
using System.Linq;
using DualityGame.Dialog;
using UnityEngine;


namespace NodeCanvas.DialogueTrees
{

    ///<summary> A DialogueActor Component.</summary>
    [AddComponentMenu("NodeCanvas/Dialogue Actor")]
    public class DialogueActor : MonoBehaviour, IDialogueActor
    {
        
        [Serializable]
        public struct MoodPortrait : IComparable<MoodPortrait>
        {
            public Mood Mood;
            public Sprite Sprite;

            int IComparable<MoodPortrait>.CompareTo(MoodPortrait other) => Mood.CompareTo(other.Mood);
        }

        [SerializeField] protected string _name;
        [SerializeField] protected List<MoodPortrait> _portraits = new();

        public string Name => _name;

        public Sprite PortraitByMood(Mood mood)
        {
            var sprite = _portraits.First(item => item.Mood == mood).Sprite;
            if (sprite == null) Debug.LogError($"Missing mood portrait for '{mood}'", this);
            return sprite;
        }

        public Transform Transform => transform;

        //IDialogueActor.transform is implemented by inherited MonoBehaviour.transform

        ///----------------------------------------------------------------------------------------------
        ///---------------------------------------UNITY EDITOR-------------------------------------------
#if UNITY_EDITOR
        private void Reset() {
            _name = gameObject.name;
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
