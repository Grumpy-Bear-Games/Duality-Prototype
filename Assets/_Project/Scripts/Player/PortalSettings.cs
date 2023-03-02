using System.Collections;
using DualityGame.Core;
using DualityGame.SaveSystem;
using DualityGame.VFX;
using Games.GrumpyBear.Core.LevelManagement;
using UnityEngine;

namespace DualityGame.Player
{
    [CreateAssetMenu(menuName = "Duality/Portal Settings", fileName = "Portal Settings", order = 0)]
    public class PortalSettings: ScriptableObject
    {
        [field: SerializeField] public GameSession GameSession { get; private set; }
        [field: SerializeField] public ScreenFader ScreenFader { get; private set; }
        [field: SerializeField] public GameState TransitionGameState { get; private set; }

        public IEnumerator PortalTo(SceneGroup destination, string spawnPointID)
        {
            var prevGameState = GameState.Current;
            TransitionGameState.SetActive();

            var moveToSpawnPoint_CO = GameSession.MoveToSpawnPoint(destination, spawnPointID);
            if (ScreenFader)
                moveToSpawnPoint_CO = ScreenFader.Wrap(moveToSpawnPoint_CO);

            yield return moveToSpawnPoint_CO;
            
            prevGameState.SetActive();
            
        }
    }
}
