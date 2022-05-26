using System.Collections;
using NodeCanvas.DialogueTrees;
using UnityEngine;

namespace DualityGame.DialogSystem.UI
{
    public class StatementAudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _localSource;

        private AudioSource _currentSource;
        private bool _fastForward;

        public void SetActor(IDialogueActor actor)
        {
            var actorSource = actor.transform != null? actor.transform.GetComponent<AudioSource>() : null;
            _currentSource = actorSource != null ? actorSource : _localSource;
        }

        public IEnumerator Play(AudioClip clip)
        {
            if (_currentSource == null)
            {
                Debug.LogError($"No AudioSource on Actor and no local source configured. Skipping audio...");
                yield break;
            }

            _currentSource.clip = clip;
            _currentSource.Play();
            _fastForward = false;
            while (_currentSource.isPlaying)
            {
                if (_fastForward){
                    _currentSource.Stop();
                    break;
                }
                yield return null;
            }
        }
        
        public void FastForward() => _fastForward = true;
    }
}
