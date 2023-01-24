using Games.GrumpyBear.Core.Events;
using UnityEngine;

namespace DualityGame.Core
{
    [CreateAssetMenu(menuName = "Duality/Events/String Event", fileName = "String Event", order = 0)]
    public class StringEvent: EventT<string> { }
}
