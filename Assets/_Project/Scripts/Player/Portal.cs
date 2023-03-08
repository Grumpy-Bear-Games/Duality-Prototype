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

        public void Trigger() => CoroutineRunner.Run(_portalSettings.PortalTo(_sceneGroup, _spawnPointID));

        private void Reset() => _portalSettings = FindObjectOfType<PortalSettings>();
    }
}
