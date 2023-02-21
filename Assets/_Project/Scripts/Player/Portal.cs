using System.Collections;
using DualityGame.Core;
using Games.GrumpyBear.Core.LevelManagement;
using UnityEngine;

namespace DualityGame.Player
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private PortalSettings _portalSettings;
        
        [Header("Destination")]
        [SerializeField] private SceneGroup _sceneGroup;
        [SerializeField] private string _spawnPointID;

        public void Trigger() => StartCoroutine(Trigger_CO());

        private IEnumerator Trigger_CO()
        {
            transform.parent = null;
            DontDestroyOnLoad(gameObject);

            var prevGameState = GameState.Current;
            _portalSettings.TransitionGameState.SetActive();

            var moveToSpawnPoint_CO = _portalSettings.GameSession.MoveToSpawnPoint(_sceneGroup, _spawnPointID);
            if (_portalSettings.ScreenFader)
                moveToSpawnPoint_CO = _portalSettings.ScreenFader.Wrap(moveToSpawnPoint_CO);

            yield return moveToSpawnPoint_CO;
            
            prevGameState.SetActive();
            
            Destroy(gameObject);
        }

        private void Reset() => _portalSettings = FindObjectOfType<PortalSettings>();
    }
}
