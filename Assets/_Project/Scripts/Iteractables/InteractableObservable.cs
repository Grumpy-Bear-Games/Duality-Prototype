using Games.GrumpyBear.Core.Observables.ScriptableObjects;
using UnityEngine;

namespace DualityGame.Iteractables
{
    [CreateAssetMenu(fileName = "Interactable Observable", menuName = "Duality/Interactable Observable", order = 0)]
    public class InteractableObservable: Observable<IInteractable> { }
}
