using System.Collections;
using Games.GrumpyBear.Core.LevelManagement;
using Games.GrumpyBear.Core.SaveSystem;
using UnityEngine;

namespace DualityGame.Core
{
    public class Bootstrapper: MonoBehaviour
    {
        [SerializeField] private SceneGroup _sceneGroup;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void LoadAllSerializableScriptableObjects()
        {
            Resources.LoadAll<SerializableScriptableObject>("");
        }

        #if !UNITY_EDITOR
        private IEnumerator Start()
        {
            yield return _sceneGroup.Load_CO();
        }
        #endif
    }
}
