using DualityGame.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace DualityGame.Playables
{
    [RequireComponent(typeof(PlayableDirector))]
    public class PlayableDirectorNotifier : NotifierBase<PlayableDirector, PlayableDirectorObservable> {}
}
