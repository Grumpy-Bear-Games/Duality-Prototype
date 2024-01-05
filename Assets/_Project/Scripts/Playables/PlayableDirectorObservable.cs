using Games.GrumpyBear.Core.Observables.ScriptableObjects;
using UnityEngine;
using UnityEngine.Playables;

namespace DualityGame.Playables
{
    [CreateAssetMenu(menuName = "Duality/PlayableDirector Observable", fileName = "PlayableDirector Observable", order = 0)]
    public class PlayableDirectorObservable: Observable<PlayableDirector> { }
}
