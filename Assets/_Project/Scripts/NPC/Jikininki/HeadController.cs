using System;
using UnityEngine;

namespace DualityGame.NPC.Jikininki
{
    public class HeadController : MonoBehaviour
    {
        private static readonly int PlayerCloseProperty = Animator.StringToHash("Head High");

        [SerializeField] private Animator _animator;
        [SerializeField] private float _maxDistance = 5f;
        
        private GameObject _player;

        public void SetPlayer(GameObject player)
        {
            _player = player;
            UpdateEnabled(Realm.Realm.Current);
        }

        private void UpdateEnabled(Realm.Realm realm) => enabled = realm != null && _player != null;

        private void Awake() => Realm.Realm.Subscribe(UpdateEnabled);
        private void OnDestroy() => Realm.Realm.Unsubscribe(UpdateEnabled);

        private void Update() => _animator.SetBool(PlayerCloseProperty, IsPlayerClose());

        private bool IsPlayerClose() => (
            (_player != null) &&
            (Realm.Realm.Current.LevelLayer == gameObject.layer) &&
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
