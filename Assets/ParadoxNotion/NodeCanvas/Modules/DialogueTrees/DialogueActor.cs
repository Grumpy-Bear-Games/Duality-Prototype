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
        public struct MoodPortrait
        {
            public Mood Mood;
            public Sprite Sprite;
        }

        [SerializeField] protected string _name;
        [SerializeField] protected List<MoodPortrait> _portraits = new();

        public string Name => _name;

        public Sprite PortraitByMood(Mood mood)
        {
            try
            {
                return _portraits.First(item => item.Mood == mood).Sprite;
            }
            catch (InvalidOperationException)
            {
                Debug.LogError($"Missing mood portrait for '{mood}'", this);
                return null;
            }
        }

        public Transform Transform => transform;

        //IDialogueActor.transform is implemented by inherited MonoBehaviour.transform

        ///----------------------------------------------------------------------------------------------
        ///---------------------------------------UNITY EDITOR-------------------------------------------
#if UNITY_EDITOR
        private void Reset() {
            _name = gameObject.name;
        }
#endif
    }
}
