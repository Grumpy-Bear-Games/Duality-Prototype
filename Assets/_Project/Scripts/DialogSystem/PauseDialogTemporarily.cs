using System.Collections;
using NodeCanvas.DialogueTrees;
using UnityEngine;

namespace DualityGame.DialogSystem
{
    public class PauseDialogTemporarily : MonoBehaviour
    {
        [SerializeField] private DialogueTreeController _dialogueTreeController;
        [SerializeField] private float _waitTime = 3f;

        public void Trigger() => StartCoroutine(Trigger_CO());

        private IEnumerator Trigger_CO()
        {
            _dialogueTreeController.graph.Pause();
            yield return new WaitForSeconds(_waitTime);
            _dialogueTreeController.graph.Resume();
        }
    }
}
