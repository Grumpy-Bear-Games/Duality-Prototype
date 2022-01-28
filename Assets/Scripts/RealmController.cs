using UnityEngine;
using UnityEngine.InputSystem;


namespace DualityGame
{
    public class RealmController : MonoBehaviour
    {
        [SerializeField] private Realm _heaven;
        [SerializeField] private Realm _hell;

        private ThirdPersonFixedController _controller;
        
        private Realm _currentRealm;
        private Realm _otherRealm => _currentRealm == _heaven ? _hell : _heaven;

        private Collider[] _colliders = new Collider[1];

        private void Awake()
        {
            _controller = GetComponent<ThirdPersonFixedController>();
            _currentRealm = _hell;
            SetActiveRealm(_heaven);
        }

        private void OnWarp(InputValue value)
        {
            if (OtherRealmBlocked())
            {
                Debug.Log("Something is blocking on the other side");
            }
            else
            {
                SetActiveRealm(_otherRealm);
            }
        }

        public bool OtherRealmBlocked()
        {
            var position = transform.position;
            var point1 = position + Vector3.up * 1.5f;
            var point2 = position + Vector3.up * 0.5f;

            return Physics.OverlapCapsuleNonAlloc(point1, point2, 0.45f, _colliders, _otherRealm.LeveLayers) > 0;
        }
        
        private void SetActiveRealm(Realm realm)
        {
            _currentRealm.Active = false;
            _currentRealm = realm;
            _currentRealm.Active = true;
            gameObject.layer = _currentRealm.PlayerLayer;
            _controller.GroundLayers = _currentRealm.LeveLayers;
        }
    }
}
