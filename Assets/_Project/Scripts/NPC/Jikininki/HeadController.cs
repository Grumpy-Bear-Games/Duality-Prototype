using UnityEngine;

namespace DualityGame.NPC.Jikininki
{
    public class HeadController : MonoBehaviour
    {
        private static readonly int PlayerCloseProperty = Animator.StringToHash("Head High");

        [SerializeField] private Animator _animator;
        [SerializeField] private float _maxDistance = 5f;
        
        private GameObject _player;
        private Realm.Realm _realm;

        public void SetPlayer(GameObject player)
        {
            _player = player;
            UpdateEnabled();
        }

        public void SetRealm(Realm.Realm realm)
        {
            _realm = realm;
            UpdateEnabled();
        }

        private void UpdateEnabled() => enabled = _realm != null && _player != null;

        private void Awake() => UpdateEnabled();
        
        private void Update() => _animator.SetBool(PlayerCloseProperty, IsPlayerClose());

        private bool IsPlayerClose() => (
            (_player != null) &&
            (_realm.LevelLayer == gameObject.layer) &&
            (Vector3.Distance(transform.position, _player.transform.position) <= _maxDistance)
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
