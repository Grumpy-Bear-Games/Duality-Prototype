using UnityEngine;
using UnityEngine.Playables;

namespace DualityGame.Playables
{
    [RequireComponent(typeof(PlayableDirector))]
    public class PlayableDirectorNotifier : MonoBehaviour
    {
        [SerializeField] private PlayableDirectorObservable _playableDirectorObservable;
        [SerializeField] private bool _registerOnAwake;
        private PlayableDirector _playableDirector;

        private void Awake()
        {
            _playableDirector = GetComponent<PlayableDirector>();
            if (!_registerOnAwake) return;
            _playableDirectorObservable.Set(_playableDirector);
        }

        private void OnEnable()
        {
            if (_registerOnAwake) return;
            _playableDirectorObservable.Set(_playableDirector);
        }

        private void OnDisable()
        {
            if (_registerOnAwake) return;
            if (_playableDirectorObservable.Value != _playableDirector) return;
            _playableDirectorObservable.Set(null);
        }

        private void OnDestroy()
        {
            if (!_registerOnAwake) return;
            if (_playableDirectorObservable.Value != _playableDirector) return;
            _playableDirectorObservable.Set(null);
        }
    }
}
