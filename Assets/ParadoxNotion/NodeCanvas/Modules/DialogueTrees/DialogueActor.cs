using UnityEngine;


namespace NodeCanvas.DialogueTrees
{

    ///<summary> A DialogueActor Component.</summary>
    [AddComponentMenu("NodeCanvas/Dialogue Actor")]
    public class DialogueActor : MonoBehaviour, IDialogueActor
    {

        [SerializeField]
        protected string _name;
        [SerializeField]
        protected Sprite _portrait;
        [SerializeField]
        protected Color _dialogueColor = Color.white;
        [SerializeField]
        protected Vector3 _dialogueOffset;


        new public string name {
            get { return _name; }
        }

        public Sprite portrait {
            get { return _portrait; }
        }

        public Color dialogueColor {
            get { return _dialogueColor; }
        }

        public Vector3 dialoguePosition {
            get { return transform.TransformPoint(_dialogueOffset); }
        }

        //IDialogueActor.transform is implemented by inherited MonoBehaviour.transform


        ///----------------------------------------------------------------------------------------------
        ///---------------------------------------UNITY EDITOR-------------------------------------------
#if UNITY_EDITOR

        void Reset() {
            _name = gameObject.name;
        }

        void OnDrawGizmos() {
            Gizmos.DrawLine(transform.position, dialoguePosition);
        }

#endif
    }
}
