using Games.GrumpyBear.Core.Observables.ScriptableObjects;
using UnityEngine;

namespace DualityGame.Core
{
    [CreateAssetMenu(fileName = "String Observable", menuName = "Duality/String Observable", order = 0)]

    public class StringObservable: Observable<string> { }
}
