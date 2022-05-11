using NodeCanvas.DialogueTrees;
using TMPro;
using UnityEngine;

namespace DualityGame.Quests
{
    [RequireComponent(typeof(TMP_Text))]
    public class DialogUIName : MonoBehaviour
    {
        private TMP_Text _name;

        private void Awake() => _name = GetComponent<TMP_Text>();

        public void SetActor(IDialogueActor actor) => _name.text = actor.name;
    }
}