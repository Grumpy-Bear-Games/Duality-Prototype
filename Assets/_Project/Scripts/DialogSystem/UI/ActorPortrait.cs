using NodeCanvas.DialogueTrees;
using UnityEngine;
using UnityEngine.UI;

namespace DualityGame.DialogSystem.UI
{
    [RequireComponent(typeof(Image))]
    public class ActorPortrait : MonoBehaviour
    {
        private Image _image;
        private void Awake() => _image = GetComponent<Image>();

        public void SetActor(IDialogueActor actor)
        {
            _image.gameObject.SetActive( actor.portraitSprite != null );
            _image.sprite = actor.portraitSprite;
        }
    }
}
