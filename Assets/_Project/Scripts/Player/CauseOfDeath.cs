using System;
using DualityGame.Core;
using UnityEngine;

namespace DualityGame.Player
{
    [CreateAssetMenu(menuName = "Duality/Cause Of Death", fileName = "Cause Of Death", order = 0)]
    public class CauseOfDeath: SerializableScriptableObject<CauseOfDeath>
    {
        public static event Action<CauseOfDeath> OnDeath;
        
        [field: SerializeField] public string Description { get; private set; }

        public void Trigger() => OnDeath?.Invoke(this);
    }
}
