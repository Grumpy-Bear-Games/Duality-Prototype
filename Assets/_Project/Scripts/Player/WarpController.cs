using DualityGame.Realm;
using DualityGame.VFX;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DualityGame.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class WarpController : MonoBehaviour
    {
        [SerializeField] private RealmObservable _currentRealm;
        [SerializeField] private Realm.Realm _heaven;
        [SerializeField] private Realm.Realm _hell;

        [Header("VFX")]
        [SerializeField] private ScreenFader _warpVFX;
        
        private CharacterController _controller;
        private readonly Collider[] _colliders = new Collider[1];

        private Realm.Realm _otherRealm => _currentRealm.Value == _heaven ? _hell : _heaven;
        
        private void Awake() => _controller = GetComponent<CharacterController>();

        [UsedImplicitly]
        private void OnWarp(InputValue value)
        {
            if (!enabled) return;
            
            if (IsOtherRealmBlocked(transform.position)) {
                Debug.Log("Something is blocking on the other side");
                return;
            }

            StartCoroutine(_warpVFX.Wrap(SwitchToOtherRealm));
        }

        private void SwitchToOtherRealm() => _currentRealm.Set(_otherRealm);

        private bool IsOtherRealmBlocked(Vector3 position)
        {
            var radius = _controller.radius;
            var point1 = position + Vector3.up * (_controller.height - radius);
            var point2 = position + Vector3.up * radius;

            return Physics.OverlapCapsuleNonAlloc(point1, point2, radius, _colliders, _otherRealm.LevelLayerMask) > 0;
        }
    }
}
