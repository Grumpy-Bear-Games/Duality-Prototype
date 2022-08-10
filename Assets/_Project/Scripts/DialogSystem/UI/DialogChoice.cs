using NodeCanvas.DialogueTrees;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DualityGame.DialogSystem.UI
{
    public class DialogChoice : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _label;

        public void Initialize(IDialogueActor actor, string text, UnityAction onClick)
        {
            _label.text = text;
            _button.onClick.AddListener(onClick);
            if (actor is ExtendedDialogActor extendedDialogActor && extendedDialogActor.Font)
            {
                _label.font = extendedDialogActor.Font;
            }
        }

        public void Select() => _button.Select();

        private void Reset()
        {
            _button = GetComponentInChildren<Button>();
            _label = GetComponentInChildren<TMP_Text>();
        }
    }
}
