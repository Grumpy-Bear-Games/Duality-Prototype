using Games.GrumpyBear.Core.Observables.ScriptableObjects;
using UnityEngine;

namespace DualityGame.Player
{
    [CreateAssetMenu(menuName = "Duality/Observable Spawn Point", fileName = "Observable Spawn Point", order = 0)]
    public class ObservableSpawnPoint : Observable<SpawnPoint>
    {
    }
}