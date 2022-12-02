using System;
using DualityGame.Realm;
using UnityEngine;

namespace DualityGame.NPC.Jikininki
{
    public class HeadController : MonoBehaviour
    {
        private static readonly int PlayerCloseProperty = Animator.StringToHash("Head High");

        [SerializeField] private Animator _animator;
        [SerializeField] private RealmObservable _currentRealm;
        [SerializeField] private float _maxDistance = 5f;
        
        private GameObject _player;

        public void SetPlayer(GameObject player)
        {
            _player = player;
            enabled = _player != null;
        }

        private void Awake() => enabled = _player != null;
        
        private void Update() => _animator.SetBool(PlayerCloseProperty, IsPlayerClose());

        private bool IsPlayerClose() => (
            (_player != null) &&
            (_currentRealm.Value.LevelLayer == gameObject.layer) &&
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
