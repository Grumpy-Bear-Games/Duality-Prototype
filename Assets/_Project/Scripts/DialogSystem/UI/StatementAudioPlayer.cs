using System.Collections;
using NodeCanvas.DialogueTrees;
using UnityEngine;

namespace DualityGame.DialogSystem.UI
{
    public class StatementAudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        private bool _fastForward;

        public IEnumerator Play(AudioClip clip)
        {
            if (_audioSource == null)
            {
                Debug.LogError($"No AudioSource on Actor and no local source configured. Skipping audio...");
                yield break;
            }

            _audioSource.clip = clip;
            _audioSource.Play();
            _fastForward = false;
            while (_audioSource.isPlaying)
            {
                if (_fastForward){
                    _audioSource.Stop();
                    break;
                }
                yield return null;
            }
        }
        
        public void FastForward() => _fastForward = true;
    }
}
