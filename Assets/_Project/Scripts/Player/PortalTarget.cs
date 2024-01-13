using System.Collections;
using DualityGame.Core;
using Games.GrumpyBear.Core.LevelManagement;
using UnityEngine;

namespace DualityGame.Player
{
    [CreateAssetMenu(menuName = "Duality/Portal Target")]
    public class PortalTarget : ScriptableObject
    {
        [SerializeField] private PortalSettings _portalSettings;

        [Header("Destination")]
        [SerializeField] private SceneGroup _sceneGroup;
        [SerializeField] private string _spawnPointID;

        public void WarpTo() => CoroutineRunner.Run(WarpTo_CO());

        private IEnumerator WarpTo_CO()
        {
            var prevGameState = GameState.Current;
            _portalSettings.TransitionGameState.SetActive();

            var moveToSpawnPoint_CO = _portalSettings.GameSession.MoveToSpawnPoint(_sceneGroup, _spawnPointID);
            if (_portalSettings.ScreenFader)
                moveToSpawnPoint_CO = _portalSettings.ScreenFader.Wrap(moveToSpawnPoint_CO);

            yield return moveToSpawnPoint_CO;

            prevGameState.SetActive();
        }
    }
}
