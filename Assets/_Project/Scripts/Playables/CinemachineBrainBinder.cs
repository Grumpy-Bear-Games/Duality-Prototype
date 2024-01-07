using Cinemachine;
using DualityGame.Utilities;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace DualityGame.Playables
{
    [RequireComponent(typeof(PlayableDirector))]
    public class CinemachineBrainBinder : MonoBehaviour
    {
        [SerializeField] private CinemachineBrainObservable _cinemachineBrainObservable;
        private PlayableDirector _playableDirector;

        private void Awake() => _playableDirector = GetComponent<PlayableDirector>();

        private void OnEnable() => _cinemachineBrainObservable.Subscribe(Subscriber);

        private void OnDisable() => _cinemachineBrainObservable.Unsubscribe(Subscriber);

        private void Subscriber(CinemachineBrain cinemachineBrain)
        {
            if (_playableDirector.playableAsset is not TimelineAsset timelineAsset) return;
            foreach (var track in timelineAsset.GetOutputTracks())
            {
                if (track is not CinemachineTrack) continue;
                _playableDirector.SetGenericBinding(track, cinemachineBrain);
                break;
            }
        }
    }
}
