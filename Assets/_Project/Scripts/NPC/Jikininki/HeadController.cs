using DualityGame.Realm;
using UnityEngine;

namespace DualityGame.NPC.Jikininki
{
    public class HeadController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private RealmObservable _currentRealm;
        [SerializeField] private float _maxDistance = 5f;
        
        private GameObject _player;
        private static readonly int PlayerCloseProperty = Animator.StringToHash("Head High");


        private void Awake() => _player = GameObject.FindWithTag("Player");

        private void Update() => _animator.SetBool(PlayerCloseProperty, IsPlayerClose());

        private bool IsPlayerClose() => (
            (_currentRealm.Value.LevelLayer == gameObject.layer) &&
            (Vector3.Distance(transform.position, _player.transform.position) <= _maxDistance)
        );

        #if UNITY_EDITOR
        private void Reset()
        {
            _animator = GetComponentInChildren<Animator>();
        }
        #endif
    }
}
