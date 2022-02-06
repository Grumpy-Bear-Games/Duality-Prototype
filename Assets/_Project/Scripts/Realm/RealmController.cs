using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DualityGame.Realm
{
    public class RealmController : MonoBehaviour
    {
        [SerializeField] private RealmObservable _currentRealm;
        [SerializeField] private Realm _heaven;
        [SerializeField] private Realm _hell;
        [SerializeField] private VolumeEffect _effect;

        private CharacterController _controller;
        
        private Realm _otherRealm => _currentRealm.Value == _heaven ? _hell : _heaven;

        private void Awake() => _controller = GetComponent<CharacterController>();

        private void OnWarp(InputValue value)
        {
            if (IsOtherRealmBlocked(transform.position)) {
                Debug.Log("Something is blocking on the other side");
                return;
            }

            StartCoroutine(CO_WarpTo(_otherRealm));
        }

        private IEnumerator CO_WarpTo(Realm realm)
        {
            yield return _effect.FadeOut();
            _currentRealm.Set(realm);
            yield return _effect.FadeIn();
        }


        private bool IsOtherRealmBlocked(Vector3 position)
        {
            var colliders = new Collider[1];
            var radius = _controller.radius;
            var point1 = position + Vector3.up * (_controller.height - radius);
            var point2 = position + Vector3.up * radius;

            return Physics.OverlapCapsuleNonAlloc(point1, point2, radius, colliders, _otherRealm.LevelLayerMask) > 0;
        }
        
       
        private void OnRealmChange(Realm newRealm) => gameObject.layer = newRealm.PlayerLayer;

        private void OnEnable()
        {
            _currentRealm.Set(_heaven);
            _currentRealm.Subscribe(OnRealmChange);
        }

        private void OnDisable() => _currentRealm.Unsubscribe(OnRealmChange);
    }
}
