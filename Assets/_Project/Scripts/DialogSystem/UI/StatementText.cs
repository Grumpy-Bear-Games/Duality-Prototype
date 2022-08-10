using System;
using System.Collections;
using Febucci.UI;
using NodeCanvas.DialogueTrees;
using TMPro;
using UnityEngine;

namespace DualityGame.DialogSystem.UI
{
    [RequireComponent(typeof(TextAnimatorPlayer))]
    public class StatementText : MonoBehaviour
    {
        [SerializeField] private float _finalDelay = 1.2f;

        private TextAnimatorPlayer _textAnimatorPlayer;
        private TMP_FontAsset _defaultFont;
        private bool _fastForward;
        public event Action OnTypeCharacter;

        private void Awake()
        {
            _textAnimatorPlayer = GetComponent<TextAnimatorPlayer>();
            _defaultFont = _textAnimatorPlayer.textAnimator.tmproText.font;
        }

        public void SetActor(IDialogueActor actor)
        {
            _textAnimatorPlayer.textAnimator.tmproText.color = actor.dialogueColor;
            if (actor is ExtendedDialogActor extendedDialogActor)
            {
                var font = extendedDialogActor.Font ? extendedDialogActor.Font : _defaultFont;
                _textAnimatorPlayer.textAnimator.tmproText.font = font;
            }
            else
            {
                _textAnimatorPlayer.textAnimator.tmproText.font = _defaultFont;
            }
        }

        public void ShowText(string text)
        {
            _textAnimatorPlayer.useTypeWriter = false;
            _textAnimatorPlayer.ShowText(text);
        }

        public IEnumerator TypeText(string text)
        {
            var dialogShown = false;
            void OnTextShowed() => dialogShown = true;
            _textAnimatorPlayer.useTypeWriter = true;
            _textAnimatorPlayer.onTextShowed.AddListener(OnTextShowed);
            _textAnimatorPlayer.ShowText(text);

            _fastForward = false;
            while (!dialogShown) {
                if (_fastForward) {
                    _textAnimatorPlayer.SkipTypewriter();
                }
                yield return null;
            }
            _textAnimatorPlayer.onTextShowed.RemoveListener(OnTextShowed);

            var finalWait = 0f;
            while (finalWait < _finalDelay && !_fastForward)
            {
                finalWait += Time.deltaTime;
                yield return null;
            }
        }
        
        private void OnEnable() => _textAnimatorPlayer.onCharacterVisible.AddListener(PlayTypeSound);

        private void OnDisable() => _textAnimatorPlayer.onCharacterVisible.RemoveListener(PlayTypeSound);

        private void PlayTypeSound(char arg0) => OnTypeCharacter?.Invoke();

        public void FastForward() => _fastForward = true;
    }
}
