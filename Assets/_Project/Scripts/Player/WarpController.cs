using DualityGame.VFX;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DualityGame.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class WarpController : MonoBehaviour
    {
        [Header("VFX")]
        [SerializeField] private ScreenFader _warpVFX;
        
        private CharacterController _controller;
        private readonly Collider[] _colliders = new Collider[1];

        private void Awake() => _controller = GetComponent<CharacterController>();

        [UsedImplicitly]
        private void OnWarp(InputValue value)
        {
            if (!enabled) return;

            if (Realm.Realm.Current.CanWarpTo == null)
            {
                Notifications.Notifications.Add("You can't warp from this realm");
                return;
            }

            if (IsOtherRealmBlocked(transform.position)) {
                Notifications.Notifications.Add("You can't warp right here. Something is blocking you on the other side");
                return;
            }
            StartCoroutine(_warpVFX.Wrap(SwitchToOtherRealm));
        }

        private static void SwitchToOtherRealm() => Realm.Realm.Current.CanWarpTo.SetActive();

        private bool IsOtherRealmBlocked(Vector3 position)
        {
            var radius = _controller.radius;
            var point1 = position + Vector3.up * (_controller.height - radius);
            var point2 = position + Vector3.up * radius;

            return Physics.OverlapCapsuleNonAlloc(point1, point2, radius, _colliders, Realm.Realm.Current.CanWarpTo.LevelLayerMask) > 0;
        }
    }
}
