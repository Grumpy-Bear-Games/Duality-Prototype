using System.Collections;
using DualityGame.Core;
using Games.GrumpyBear.Core.LevelManagement;
using UnityEngine;

namespace DualityGame.Player
{
    [CreateAssetMenu(menuName = "Duality/Spawn Point Reference")]
    public class SpawnPointReference : ScriptableObject
    {
        [field: Header("Destination")]
        [field: SerializeField] public SceneGroup SceneGroup { get; private set; }
        [field: SerializeField] public Realm.Realm Realm { get; private set; }


        public void SpawnAt(SpawnSettings spawnSettings) => CoroutineRunner.Run(WarpTo_CO(spawnSettings));

        private IEnumerator WarpTo_CO(SpawnSettings spawnSettings)
        {
            var prevGameState = GameState.Current;
            spawnSettings.TransitionGameState.SetActive();

            var moveToSpawnPoint_CO = spawnSettings.GameSession.MoveToSpawnPoint(this);
            if (spawnSettings.ScreenFader)
                moveToSpawnPoint_CO = spawnSettings.ScreenFader.Wrap(moveToSpawnPoint_CO);

            yield return moveToSpawnPoint_CO;

            prevGameState.SetActive();
        }
    }
}
