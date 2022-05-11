using NodeCanvas.DialogueTrees;
using UnityEngine;
using UnityEngine.UI;

namespace DualityGame.Quests
{
    [RequireComponent(typeof(Image))]
    public class DialogUIPortrait : MonoBehaviour
    {
        private Image _image;
        private void Awake() => _image = GetComponent<Image>();

        protected internal void SetActor(IDialogueActor actor)
        {
            _image.gameObject.SetActive( actor.portraitSprite != null );
            _image.sprite = actor.portraitSprite;
        }
    }
}
