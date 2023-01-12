using DualityGame.Dialog;
using UnityEngine;


namespace NodeCanvas.DialogueTrees
{

    ///<summary> A DialogueActor Component.</summary>
    [AddComponentMenu("NodeCanvas/Dialogue Actor")]
    public class DialogueActor : MonoBehaviour, IDialogueActor
    {
        [SerializeField] private ActorAsset _actorAsset;

        public string Name => _actorAsset.Name;

        public Sprite PortraitByMood(Mood mood) => _actorAsset.PortraitByMood(mood);

        public Transform Transform => transform;

        //IDialogueActor.transform is implemented by inherited MonoBehaviour.transform
    }
}
