using UnityEngine;


namespace NodeCanvas.DialogueTrees
{

    ///<summary> A DialogueActor Component.</summary>
    [AddComponentMenu("NodeCanvas/Dialogue Actor")]
    public class DialogueActor : MonoBehaviour, IDialogueActor
    {
        [SerializeField] private ActorAsset _actorAsset;

        public new string name => _actorAsset.Name;

        public Sprite PortraitByMood(Mood mood) => _actorAsset.PortraitByMood(mood);

        //IDialogueActor.transform is implemented by inherited MonoBehaviour.transform
    }
}
