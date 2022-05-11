using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DualityGame.Quests
{
    [RequireComponent(typeof(DialogUITalk))]
    public class DialogUITypingSFX : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private List<AudioClip> _typingSounds;

        private DialogUITalk _dialog;
        
        private void Awake() => _dialog = GetComponent<DialogUITalk>();

        private void OnEnable() => _dialog.OnTypeCharacter += PlayTypeSound;

        private void OnDisable() => _dialog.OnTypeCharacter -= PlayTypeSound;

        private void PlayTypeSound()
        {
            if (_typingSounds.Count <= 0) return;
            var sound = _typingSounds[ Random.Range(0, _typingSounds.Count) ];
            if (sound != null){
                _audioSource.PlayOneShot(sound, Random.Range(0.6f, 1f));
            }
        }
        
    }
}
