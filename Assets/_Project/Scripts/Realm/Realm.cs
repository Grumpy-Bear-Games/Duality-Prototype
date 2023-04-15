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
        
        [field: Header("Lighting")]
        [field: SerializeField] public Material SkyboxMaterial { get; private set;  }
        [field: SerializeField] public Color LightColor { get; private set;  }
        [field: SerializeField][field: Range(0f, 10f)] public float LightIntensity { get; private set; } = 1f;
        
        public int LevelLayerMask => 1 << LevelLayer;
    }
}
