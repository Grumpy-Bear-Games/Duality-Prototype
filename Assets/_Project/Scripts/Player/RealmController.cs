using System.Collections;
using DualityGame.Core;
using DualityGame.Realm;
using DualityGame.VFX;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DualityGame.Player
{
    public class RealmController : MonoBehaviour, IKillable
    {
        [SerializeField] private RealmObservable _currentRealm;
        [SerializeField] private Realm.Realm _heaven;
        [SerializeField] private Realm.Realm _hell;
        [SerializeField] private Inventory.Inventory _inventory;

        [Header("VFX")]
        [SerializeField] private VFXFadeTrigger _warpVFX;
        
        [Header("Respawn")]
        [SerializeField] private Transform _respawnPoint;

        private CharacterController _controller;
        
        private Realm.Realm _otherRealm => _currentRealm.Value == _heaven ? _hell : _heaven;

        public void Kill()
        {
            _controller.enabled = false;
            transform.position = _respawnPoint.position;
            _controller.enabled = true;
            _currentRealm.Set(_heaven);
            var item = _inventory.TakeItem();
            if (item != null) item.ReturnToInitialPosition();
        }

        private void Awake() => _controller = GetComponent<CharacterController>();

        [UsedImplicitly]
        private void OnWarp(InputValue value)
        {
            if (IsOtherRealmBlocked(transform.position)) {
                Debug.Log("Something is blocking on the other side");
                return;
            }

            StartCoroutine(CO_WarpTo(_otherRealm));
        }

        private IEnumerator CO_WarpTo(Realm.Realm realm)
        {
            yield return _warpVFX.Execute(IVFXFadeEffect.Direction.FadeOut);
            _currentRealm.Set(realm);
            yield return _warpVFX.Execute(IVFXFadeEffect.Direction.FadeIn);
        }

        private bool IsOtherRealmBlocked(Vector3 position)
        {
            var colliders = new Collider[1];
            var radius = _controller.radius;
            var point1 = position + Vector3.up * (_controller.height - radius);
            var point2 = position + Vector3.up * radius;

            return Physics.OverlapCapsuleNonAlloc(point1, point2, radius, colliders, _otherRealm.LevelLayerMask) > 0;
        }
        
       
        private void OnRealmChange(Realm.Realm newRealm) => gameObject.layer = newRealm.PlayerLayer;

        private void OnEnable()
        {
            _currentRealm.Set(_heaven);
            _currentRealm.Subscribe(OnRealmChange);
        }

        private void OnDisable() => _currentRealm.Unsubscribe(OnRealmChange);
    }
}
