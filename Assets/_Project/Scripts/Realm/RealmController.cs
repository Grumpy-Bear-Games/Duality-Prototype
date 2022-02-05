using DualityGame.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DualityGame.Realm
{
    public class RealmController : MonoBehaviour
    {
        [SerializeField] private RealmObservable _currentRealm;
        [SerializeField] private Realm _heaven;
        [SerializeField] private Realm _hell;
        
        
        private ThirdPersonFixedController _controller;
        private Realm _otherRealm => _currentRealm.Value == _heaven ? _hell : _heaven;


        private void Awake() => _controller = GetComponent<ThirdPersonFixedController>();


        private void OnWarp(InputValue value)
        {
            if (IsOtherRealmBlocked(transform.position)) {
                Debug.Log("Something is blocking on the other side");
                return;
            }
            
            _currentRealm.Set(_otherRealm);
        }


        private bool IsOtherRealmBlocked(Vector3 position)
        {
            var colliders = new Collider[1];
            var point1 = position + Vector3.up * 1.5f;
            var point2 = position + Vector3.up * 0.5f;

            return Physics.OverlapCapsuleNonAlloc(point1, point2, 0.45f, colliders, _otherRealm.LevelLayer) > 0;
        }
        
       
        private void OnRealmChange(Realm newRealm)
        {
            gameObject.layer = newRealm.PlayerLayer;
            _controller.GroundLayers = 1 << newRealm.LevelLayer;
        }

        private void OnEnable()
        {
            _currentRealm.Set(_heaven);
            _currentRealm.Subscribe(OnRealmChange);
        }

        private void OnDisable() => _currentRealm.Unsubscribe(OnRealmChange);
    }
}
