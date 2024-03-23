using System;
using UnityEngine;

namespace NodeCanvas.DialogueTrees
{
    ///<summary>A basic rather limited implementation of IDialogueActor</summary>
    [Serializable]
    public class ProxyDialogueActor : IDialogueActor
    {
        public string Name { get; }

        public Sprite PortraitByMood(Mood mood) => null;

        public Transform Transform { get; }

        public ProxyDialogueActor(string name, Transform transform) {
            Name = name;
            Transform = transform;
        }
    }
}
