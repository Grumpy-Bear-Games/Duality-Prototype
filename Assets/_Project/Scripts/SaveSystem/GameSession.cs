using System;
using System.Collections;
using System.Collections.Generic;
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
        
        private const string SaveFileName = "Game";

        private SceneGroup _sceneGroup;
        private string _lastSpawnPointID;
        private Dictionary<string, Dictionary<string, object>> _entityStates = new();

        public IEnumerator NewGame()
        {
            FileSystem.Delete(SaveFileName);
            ClearSession();
            
            yield return _sceneGroup.Load_CO();
            SpawnController.Instance.MoveToSpawnPoint(null);
        }

        public IEnumerator LoadGame()
        {
            LoadFromFile();
            yield return _sceneGroup.Load_CO();
            RestoreState();
            SpawnController.Instance.MoveToSpawnPoint(_lastSpawnPointID);
        }

        public void SaveGame()
        {
            CaptureState();
            SaveToFile();
        }

        public IEnumerator Respawn()
        {
            yield return MoveToSpawnPoint(_firstSceneGroup, null);
            SaveToFile();
        }

        public IEnumerator MoveToSpawnPoint(SceneGroup sceneGroup, string spawnPointID)
        {
            _sceneGroup = sceneGroup;
            _lastSpawnPointID = spawnPointID; 
            CaptureState();
            
            yield return _sceneGroup.Load_CO();
            RestoreState();
            SpawnController.Instance.MoveToSpawnPoint(_lastSpawnPointID);
        }
        
        public void CaptureState() => SaveableEntity.CaptureEntityStates(_entityStates);
        public void RestoreState() => SaveableEntity.RestoreEntityStates(_entityStates);

        private void ClearSession()
        {
            _entityStates = new();
            _lastSpawnPointID = null;
            _sceneGroup = _firstSceneGroup;
        }

        public bool HasSaveFile() => FileSystem.Exists(SaveFileName);

        [Serializable]
        private class SerializableSession
        {
            public readonly ObjectGuid SceneGroupID;
            public readonly string LastSpawnPointID;
            public readonly Dictionary<string, Dictionary<string, object>> EntityStates;
            
            public SceneGroup SceneGroup => Games.GrumpyBear.Core.LevelManagement.SceneGroup.GetByGuid(SceneGroupID);

            public SerializableSession(SceneGroup sceneGroup, string lastSpawnPointID, Dictionary<string, Dictionary<string, object>> entityStates)
            {
                // TODO: Notice this is a broken hack, just to make it work for now. We need an actual solution.
                SceneGroupID = sceneGroup.ObjectGuid;
                LastSpawnPointID = lastSpawnPointID;
                EntityStates = entityStates;
            }
        }

        private void SaveToFile()
        {
            var serializableSession = new SerializableSession(_sceneGroup, _lastSpawnPointID, _entityStates);
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
                _lastSpawnPointID = serializableSession.LastSpawnPointID;
                _entityStates = serializableSession.EntityStates;
            }
        }
    }
}
