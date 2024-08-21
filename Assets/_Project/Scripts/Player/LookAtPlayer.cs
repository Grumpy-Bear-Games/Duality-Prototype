using Unity.Cinemachine;
using UnityEngine;

namespace DualityGame.Player
{
    [RequireComponent(typeof(CinemachineCamera))]
    public sealed class LookAtPlayer: MonoBehaviour
    {
        private CinemachineCamera _camera;
        private void Awake() => _camera = GetComponent<CinemachineCamera>();
        private void OnEnable() => ServiceLocator.ServiceLocator.Subscribe<Player>(OnPlayerRegistered);
        private void OnDisable() => ServiceLocator.ServiceLocator.Unsubscribe<Player>(OnPlayerRegistered);
        private void OnPlayerRegistered(Player player)
        {
            var playerTransform = player?.transform;
            //_camera.LookAt = playerTransform;
            _camera.Follow = playerTransform;
        }
    }
}
