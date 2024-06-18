using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DualityGame
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class DelayedUnload : MonoBehaviour
    {
        private Scene _myScene;
        private CinemachineVirtualCamera _camera;

        private void Awake()
        {
            _myScene = gameObject.scene;
            _camera = GetComponent<CinemachineVirtualCamera>();
            DontDestroyOnLoad(gameObject);
            gameObject.name = $"[{_myScene.name}] {gameObject.name}";
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnSceneUnloaded(Scene scene)
        {
            if (_myScene.path != scene.path) return;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            StartCoroutine(DestroyAfterBlend_CO());
        }

        private IEnumerator DestroyAfterBlend_CO()
        {
            if (!ServiceLocator.ServiceLocator.TryGet<CinemachineBrain>(out var brain)) Destroy(gameObject);
            _camera.Priority = -1;
            while (ReferenceEquals(brain.ActiveVirtualCamera, _camera) ||
                   ReferenceEquals(brain.ActiveBlend?.CamA, _camera))
            {
                yield return null;
            }
            Destroy(gameObject);
        }
    }
}
