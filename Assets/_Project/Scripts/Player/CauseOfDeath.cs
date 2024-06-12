using Games.GrumpyBear.Core.SaveSystem;
using UnityEngine;

namespace DualityGame.Player
{
    [CreateAssetMenu(menuName = "Duality/Cause Of Death", fileName = "Cause Of Death", order = 0)]
    public class CauseOfDeath: SerializableScriptableObject<CauseOfDeath>
    {
        [field: SerializeField] public string Description { get; private set; }
    }
}
