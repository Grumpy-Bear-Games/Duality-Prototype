using UnityEngine;
using UnityEngine.InputSystem;

namespace DualityGame.Realm
{
    public class RealmController : MonoBehaviour
    {
        [SerializeField] private RealmManager _realmManager;
        
        private ThirdPersonFixedController _controller;

        private void Awake()
        {
            _controller = GetComponent<ThirdPersonFixedController>();
        }


        private void OnWarp(InputValue value)
        {
            if (_realmManager.IsOtherRealmBlocked(transform.position))
            {
                Debug.Log("Something is blocking on the other side");
            }
            else
            {
                _realmManager.Warp();
            }
        }
       
        private void OnRealmChange(Realm newRealm)
        {
            gameObject.layer = newRealm.PlayerLayer;
            _controller.GroundLayers = newRealm.LevelLayer;
        }

        private void OnEnable() => _realmManager.CurrentRealm.Subscribe(OnRealmChange);
        private void OnDisable() => _realmManager.CurrentRealm.Unsubscribe(OnRealmChange);
    }
}
