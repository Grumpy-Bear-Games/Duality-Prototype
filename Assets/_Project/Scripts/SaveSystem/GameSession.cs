using System;
using System.Collections;
using System.Collections.Generic;
using DualityGame.Core;
using DualityGame.Player;
using Games.GrumpyBear.Core.LevelManagement;
using Games.GrumpyBear.Core.SaveSystem;
using UnityEngine;

namespace DualityGame.SaveSystem
{
    [CreateAssetMenu(menuName = "Duality/Game Session", fileName = "Game Session", order = 0)]
    public class GameSession : ScriptableObject
    {
        [SerializeField] private SceneGroup _firstSceneGroup;
        [SerializeField] private GameState _firstGameState;
        
        private const string SaveFileName = "Game";

        private SceneGroup _sceneGroup;
        private Dictionary<string, Dictionary<string, object>> _entityStates = new();

        public IEnumerator NewGame()
        {
            FileSystem.Delete(SaveFileName);
            ClearSession();
            
            yield return _firstSceneGroup.Load_CO();
            SpawnController.Instance.MoveToSpawnPoint(null);
            _firstGameState.SetActive();
        }

        public IEnumerator LoadGame()
        {
            LoadFromFile();
            yield return _sceneGroup.Load_CO();
            RestoreState();
            _firstGameState.SetActive();
        }

        public void SaveGame()
        {
            CaptureState();
            SaveToFile();
        }

        public IEnumerator Respawn()
        {
            yield return MoveToSpawnPoint(_firstSceneGroup, null);
            CaptureState();
            SaveToFile();
        }

        public IEnumerator MoveToSpawnPoint(SceneGroup sceneGroup, string spawnPointID)
        {
            CaptureState();
            
            yield return sceneGroup.Load_CO();
            RestoreState();
            SpawnController.Instance.MoveToSpawnPoint(spawnPointID);
        }
        
        public void CaptureState() => SaveableEntity.CaptureEntityStates(_entityStates);
        public void RestoreState() => SaveableEntity.RestoreEntityStates(_entityStates);

        private void ClearSession()
        {
            _entityStates = new Dictionary<string, Dictionary<string, object>>();
            _sceneGroup = _firstSceneGroup;
        }

        public bool HasSaveFile => FileSystem.Exists(SaveFileName);

        [Serializable]
        private class SerializableSession
        {
            public readonly ObjectGuid SceneGroupID;
            public readonly Dictionary<string, Dictionary<string, object>> EntityStates;
            
            public SceneGroup SceneGroup => SceneGroup.GetByGuid(SceneGroupID);

            public SerializableSession(SceneGroup sceneGroup, Dictionary<string, Dictionary<string, object>> entityStates)
            {
                SceneGroupID = sceneGroup.ObjectGuid;
                EntityStates = entityStates;
            }
        }

        private void SaveToFile()
        {
            var serializableSession = new SerializableSession(_sceneGroup ? _sceneGroup : _firstSceneGroup, _entityStates);
            FileSystem.SaveFile(SaveFileName, serializableSession);
        }

        private void LoadFromFile()
        {
            var serializableSession = FileSystem.LoadFile<SerializableSession>(SaveFileName);
            if (serializableSession == null)
            {
                ClearSession();
            }
            else
            {
                _sceneGroup = serializableSession.SceneGroup;
                _entityStates = serializableSession.EntityStates;
            }
        }

        private void OnEnable() => SceneManager.CurrentSceneGroup.OnChange += OnSceneGroupChanged;

        private void OnDisable() => SceneManager.CurrentSceneGroup.OnChange -= OnSceneGroupChanged;

        private void OnSceneGroupChanged(SceneGroup sceneGroup) => _sceneGroup = sceneGroup;
    }
}
