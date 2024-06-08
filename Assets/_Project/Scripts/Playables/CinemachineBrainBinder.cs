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
        private void OnEnable() => ServiceLocator.Subscribe<CinemachineBrain>(OnCinemachineBrainRegistered);
        private void OnDisable() => ServiceLocator.Unsubscribe<CinemachineBrain>(OnCinemachineBrainRegistered);
        private void OnCinemachineBrainRegistered(CinemachineBrain cinemachineBrain)
        {
            var playableDirector = GetComponent<PlayableDirector>();
            if (playableDirector.playableAsset is not TimelineAsset timelineAsset) return;
            foreach (var track in timelineAsset.GetOutputTracks())
            {
                if (track is not CinemachineTrack) continue;
                playableDirector.SetGenericBinding(track, cinemachineBrain);
                break;
            }
        }
    }
}
