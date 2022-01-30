using Games.GrumpyBear.Core.Observables;
using UnityEngine;

namespace DualityGame.Realm
{
    [CreateAssetMenu(fileName = "Realm Manager", menuName = "Duality/Realm Manager", order = 0)]
    public class RealmManager : ScriptableObject
    {
        [SerializeField] private Realm _heaven;
        [SerializeField] private Realm _hell;

        public IReadonlyObservable<Realm> CurrentRealm => _currentRealm;
        private readonly Observable<Realm> _currentRealm = new();

        private Realm _otherRealm => _currentRealm.Value == _heaven ? _hell : _heaven;
        
        public bool IsOtherRealmBlocked(Vector3 position)
        {
            var colliders = new Collider[1];
            var point1 = position + Vector3.up * 1.5f;
            var point2 = position + Vector3.up * 0.5f;

            return Physics.OverlapCapsuleNonAlloc(point1, point2, 0.45f, colliders, _otherRealm.LevelLayer) > 0;
        }

        public void Warp() => _currentRealm.Set(_otherRealm);

        private void OnEnable() => _currentRealm.Set(_heaven);
    }
}
