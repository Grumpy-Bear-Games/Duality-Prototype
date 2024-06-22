using System.Collections;
using DualityGame.VFX;
using UnityEngine;

namespace DualityGame.Warp
{
    public class WarpManager : MonoBehaviour
    {
        [Header("Timing")]
        [SerializeField] private AnimationCurve _warpRadius;
        [SerializeField] private AnimationCurve _transition;
        [SerializeField, Range(0.3f, 2f)] private float _duration = 1.8f;
        [SerializeField] private float _shockwaveRange = 150f;


        public static WarpManager Instance { get; private set; }

        private bool _skipNextRealmChange;
        private Coroutine _ongoingWarp;
        public bool IsWarping { get; private set; } = false;

        private void Awake() => Instance = this;

        private void OnEnable() => Realm.Realm.Subscribe(OnRealmChanged);
        private void OnDisable() => Realm.Realm.Unsubscribe(OnRealmChanged);

        public void WarpTo(Realm.Realm realm, Vector3 center)
        {
            if (IsWarping) return;
            _ongoingWarp = StartCoroutine(WarpTo_CO(realm, center));
        }

        private IEnumerator WarpTo_CO(Realm.Realm realm, Vector3 center)
        {
            IsWarping = true;
            ShaderGlobals.WarpCenter = center;
            ShaderGlobals.WarpToRealm = realm.LevelLayer;
            _skipNextRealmChange = true;
            realm.SetActive();

            var t = 0f;
            while (t < 1f)
            {
                ShaderGlobals.WarpRadius = _warpRadius.Evaluate(t) * _shockwaveRange;
                ShaderGlobals.WarpTransition = _transition.Evaluate(t);
                yield return null;
                t += Time.deltaTime / _duration;
            }
            ShaderGlobals.CurrentRealm = realm.LevelLayer;
            ShaderGlobals.WarpToRealm = realm.CanWarpTo.LevelLayer;
            ShaderGlobals.WarpRadius = 0f;
            ShaderGlobals.WarpTransition = 0f;
            IsWarping = false;
        }

        private void OnRealmChanged(Realm.Realm realm)
        {
            if (_skipNextRealmChange)
            {
                _skipNextRealmChange = false;
                return;
            }

            if (IsWarping)
            {
                StopCoroutine(_ongoingWarp);
                _ongoingWarp = null;
            }

            if (realm == null) return;
            ShaderGlobals.WarpEffectEnabled = true;
            ShaderGlobals.CurrentRealm = realm.LevelLayer;
            ShaderGlobals.WarpToRealm = realm.CanWarpTo != null ? realm.CanWarpTo.LevelLayer : -1;
            ShaderGlobals.WarpRadius = 0f;
            ShaderGlobals.WarpTransition = 0f;
        }
    }
}
