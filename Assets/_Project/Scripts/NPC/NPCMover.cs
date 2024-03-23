using System.Collections.Generic;
using System.Linq;
using DualityGame.Quests;
using Games.GrumpyBear.Core.LevelManagement;
using UnityEngine;

namespace DualityGame.NPC
{
    public class NPCMover : MonoBehaviour
    {
        [SerializeField] private SceneGroup _startingSceneGroup;
        [SerializeField] private List<Entry> _checkpoints = new();

        private void Awake()
        {
            SceneManager.CurrentSceneGroup.Subscribe(CheckLocation);
            Realm.Realm.Subscribe(OnRealmChange);
        }

        private void OnDestroy()
        {
            Realm.Realm.Unsubscribe(OnRealmChange);
            SceneManager.CurrentSceneGroup.Unsubscribe(CheckLocation);
        }

        private void OnRealmChange(Realm.Realm realm) => CheckLocation(SceneManager.CurrentSceneGroup.Value);

        private void CheckLocation(SceneGroup currentSceneGroup)
        {
            if (currentSceneGroup == null) return;
            var currentLocation = _checkpoints
                .TakeWhile(checkpoint => checkpoint._checkpoint.Reached)
                .Select(entry => entry._sceneGroup)
                .DefaultIfEmpty(_startingSceneGroup)
                .LastOrDefault();
            gameObject.SetActive(currentSceneGroup == currentLocation);
        }

        [System.Serializable]
        public struct Entry
        {
            [SerializeField] public Checkpoint _checkpoint;
            [SerializeField] public SceneGroup _sceneGroup;
        }
    }
}
