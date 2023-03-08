using Games.GrumpyBear.Core.Observables.ScriptableObjects;
using UnityEngine;

namespace DualityGame.Realm
{
    [CreateAssetMenu(fileName = "Realm", menuName = "Duality/Realm", order = 0)]
    public class Realm : GlobalStateT<Realm>
    {
        [field: SerializeField] public int LevelLayer { get; private set;  }
        [field: SerializeField] public int PlayerLayer { get; private set;  }
        [field: SerializeField] public Realm CanWarpTo { get; private set;  }

        public int LevelLayerMask => 1 << LevelLayer;
    }
}
