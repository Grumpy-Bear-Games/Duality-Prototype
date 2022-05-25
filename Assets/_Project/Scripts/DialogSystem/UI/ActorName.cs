using NodeCanvas.DialogueTrees;
using TMPro;
using UnityEngine;

namespace DualityGame.DialogSystem.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class ActorName : MonoBehaviour
    {
        private TMP_Text _name;

        private void Awake() => _name = GetComponent<TMP_Text>();

        public void SetActor(IDialogueActor actor) => _name.text = actor.name;
    }
}
