using Cinemachine;
using UnityEngine;

namespace DualityGame.Player
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public sealed class LookAtPlayer: MonoBehaviour
    {
        private CinemachineVirtualCamera _camera;
        private void Awake() => _camera = GetComponent<CinemachineVirtualCamera>();
        private void OnEnable() => ServiceLocator.ServiceLocator.Subscribe<Player>(OnPlayerRegistered);
        private void OnDisable() => ServiceLocator.ServiceLocator.Unsubscribe<Player>(OnPlayerRegistered);
        private void OnPlayerRegistered(Player player)
        {
            var playerTransform = player?.transform;
            _camera.LookAt = playerTransform;
            _camera.Follow = playerTransform;
        }
    }
}
