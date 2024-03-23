using UnityEngine;

namespace NodeCanvas.DialogueTrees
{

    ///<summary> An interface to use for DialogueActors within a DialogueTree.</summary>
	public interface IDialogueActor
    {
        string name { get; }
        Sprite PortraitByMood(Mood mood);
        Transform transform { get; }
    }

    ///<summary>A basic rather limited implementation of IDialogueActor</summary>
    [System.Serializable]
    public class ProxyDialogueActor : IDialogueActor
    {
        private string _name;
        private Transform _transform;

        public string name {
            get { return _name; }
        }
        public Sprite PortraitByMood(Mood mood) => null;

        public Transform transform {
            get { return _transform; }
        }

        public ProxyDialogueActor(string name, Transform transform) {
            this._name = name;
            this._transform = transform;
        }
    }

}
