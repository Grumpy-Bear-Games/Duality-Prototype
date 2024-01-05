using Cinemachine;
using Games.GrumpyBear.Core.Observables.ScriptableObjects;
using UnityEngine;

namespace DualityGame.Utilities
{
    [CreateAssetMenu(menuName = "Duality/CinemachineBrain Observable", fileName = "CinemachineBrain Observable", order = 0)]
    public class CinemachineBrainObservable: Observable<CinemachineBrain> { }
}
