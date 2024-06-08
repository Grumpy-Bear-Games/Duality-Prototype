using UnityEngine;

namespace DualityGame.NPC.Jikininki
{
    public sealed class HeadController : MonoBehaviour
    {
        private static readonly int PlayerCloseProperty = Animator.StringToHash("Head High");

        [SerializeField] private Animator _animator;
        [SerializeField] private float _maxDistance = 5f;
        
        private Transform _playerTransform;

        private void OnEnable() => ServiceLocator.ServiceLocator.Subscribe<Player.Player>(OnPlayerRegistered);
        private void OnDisable() => ServiceLocator.ServiceLocator.Unsubscribe<Player.Player>(OnPlayerRegistered);
        private void OnPlayerRegistered(Player.Player player) => _playerTransform = player?.transform;

        private void Update() => _animator.SetBool(PlayerCloseProperty, IsPlayerClose());

        private bool IsPlayerClose() => (
            (_playerTransform is not null) &&
            (Realm.Realm.Current?.LevelLayer == gameObject.layer) &&
            (Vector3.Distance(transform.position, _playerTransform.position) <= _maxDistance)
        );
        
        #if UNITY_EDITOR
        private void Reset() => _animator = GetComponentInChildren<Animator>();

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _maxDistance);
        }
        #endif
    }
}
