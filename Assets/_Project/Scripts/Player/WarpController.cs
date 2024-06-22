using System;
using DualityGame.Warp;
using Games.GrumpyBear.Core.Observables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DualityGame.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class WarpController : MonoBehaviour
    {
        private static readonly Observable<WarpState> _warpState = new();
        public static IReadonlyObservable<WarpState> State => _warpState;


        [SerializeField] private float _verticalOffset = 0.01f;
        [SerializeField] private InputActionReference _warpAction;

        private CharacterController _controller;
        private readonly Collider[] _colliders = new Collider[1];

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _warpAction.action.performed += OnWarp;
        }

        private void OnEnable() => _warpAction.action.Enable();
        private void OnDisable() => _warpAction.action.Disable();

        private void OnDestroy() => _warpAction.action.performed -= OnWarp;

        private void Update() => UpdateWarpState();

        private void UpdateWarpState()
        {
            if (WarpManager.Instance.IsWarping) {
                _warpState.Set(WarpState.IsWarping);
                return;
            }

            if (Realm.Realm.Current == null || Realm.Realm.Current.CanWarpTo == null)
            {
                _warpState.Set(WarpState.NoRealmToWarpTo);
                return;
            }

            if (IsOtherRealmBlocked(transform.position))
            {
                _warpState.Set(WarpState.Blocked);
                return;
            }

            _warpState.Set(WarpState.CanWarp);
        }

        private void OnWarp(InputAction.CallbackContext callbackContext)
        {
            if (!enabled) return;

            switch (_warpState.Value)
            {
                case WarpState.CanWarp:
                    WarpManager.Instance.WarpTo(Realm.Realm.Current.CanWarpTo, transform.position);
                    break;
                case WarpState.NoRealmToWarpTo:
                    Notifications.Notifications.Add("You can't warp from this realm");
                    break;
                case WarpState.Blocked:
                    Notifications.Notifications.Add("You can't warp right here. Something is blocking you on the other side");
                    break;
                case WarpState.IsWarping:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool IsOtherRealmBlocked(Vector3 position)
        {
            var radius = _controller.radius;
            var point1 = position + Vector3.up * (_controller.height - radius + _verticalOffset);
            var point2 = position + Vector3.up * (radius + _verticalOffset);

            return Physics.OverlapCapsuleNonAlloc(point1, point2, radius, _colliders, Realm.Realm.Current.CanWarpTo.LevelLayerMask) > 0;
        }

        public enum WarpState
        {
            CanWarp,
            IsWarping,
            NoRealmToWarpTo,
            Blocked,
        }
    }
}
