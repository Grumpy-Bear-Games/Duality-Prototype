using Games.GrumpyBear.Core.Events;
using UnityEngine;

namespace DualityGame.Player
{
    [CreateAssetMenu(menuName = "Duality/Kill Event", fileName = "Kill Event", order = 0)]
    public class KillEvent : EventT<CauseOfDeath>{}
}
