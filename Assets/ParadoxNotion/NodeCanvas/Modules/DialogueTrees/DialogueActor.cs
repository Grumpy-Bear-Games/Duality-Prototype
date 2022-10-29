using UnityEngine;


namespace NodeCanvas.DialogueTrees
{

    ///<summary> A DialogueActor Component.</summary>
    [AddComponentMenu("NodeCanvas/Dialogue Actor")]
    public class DialogueActor : MonoBehaviour, IDialogueActor
    {

        [SerializeField] protected string _name;
        [SerializeField] protected Sprite _portrait;

        public string Name => _name;

        public Sprite Portrait => _portrait;

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
