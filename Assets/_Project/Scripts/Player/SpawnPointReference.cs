using System.Collections;
using DualityGame.Core;
using Games.GrumpyBear.Core.LevelManagement;
using UnityEngine;

namespace DualityGame.Player
{
    [CreateAssetMenu(menuName = "Duality/Spawn Point Reference")]
    public class SpawnPointReference : ScriptableObject
    {
        [Header("Destination")]
        [SerializeField] private SceneGroup _sceneGroup;

        public SceneGroup SceneGroup => _sceneGroup;

        public void WarpTo(PortalSettings portalSettings) => CoroutineRunner.Run(WarpTo_CO(portalSettings));

        private IEnumerator WarpTo_CO(PortalSettings portalSettings)
        {
            var prevGameState = GameState.Current;
            portalSettings.TransitionGameState.SetActive();

            var moveToSpawnPoint_CO = portalSettings.GameSession.MoveToSpawnPoint(this);
            if (portalSettings.ScreenFader)
                moveToSpawnPoint_CO = portalSettings.ScreenFader.Wrap(moveToSpawnPoint_CO);

            yield return moveToSpawnPoint_CO;

            prevGameState.SetActive();
        }
    }
}
