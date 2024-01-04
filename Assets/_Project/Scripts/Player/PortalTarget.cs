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

        public void WarpTo() => CoroutineRunner.Run(_portalSettings.PortalTo(_sceneGroup, _spawnPointID));
    }
}
