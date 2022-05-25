using NodeCanvas.DialogueTrees;
using TMPro;
using UnityEngine;

namespace DualityGame.DialogSystem
{
    public class ExtendedDialogActor: DialogueActor
    {
        [SerializeField] private TMP_FontAsset _font;

        public TMP_FontAsset Font => _font;
    }
}
