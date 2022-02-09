using Games.GrumpyBear.Core.Observables.ScriptableObjects;
using UnityEngine;

namespace DualityGame.Core
{
    [CreateAssetMenu(fileName = "Play State", menuName = "Duality/Play State", order = 0)]
    public class PlayState: Observable<PlayState.State>
    {
        public enum State {
            Moving,
            Talking,
        }
    }
}
