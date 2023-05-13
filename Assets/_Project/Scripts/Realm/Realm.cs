using Games.GrumpyBear.Core.Observables.ScriptableObjects;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
#endif

namespace DualityGame.Realm
{
    [CreateAssetMenu(fileName = "Realm", menuName = "Duality/Realm", order = 0)]
    public class Realm : GlobalStateT<Realm>
    {
        [field: SerializeField] public int LevelLayer { get; private set;  }
        [field: SerializeField] public int PlayerLayer { get; private set;  }
        [field: SerializeField] public Realm CanWarpTo { get; private set;  }

        public int LevelLayerMask => 1 << LevelLayer;

        #if UNITY_EDITOR
        public static Realm FromLayer(int layer) =>
            AssetDatabase.FindAssets($"t:{nameof(Realm)}")
                .ToList()
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<Realm>)
                .FirstOrDefault(realm => realm.LevelLayer == layer);
        #endif
    }
}
