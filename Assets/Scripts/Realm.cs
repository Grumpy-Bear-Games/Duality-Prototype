using UnityEngine;

namespace DualityGame
{
    public class Realm : MonoBehaviour
    {
        [SerializeField] private LayerMask _levelLayers;
        [SerializeField] private int _playerLayer;

        public LayerMask LeveLayers => _levelLayers;
        public int PlayerLayer => _playerLayer;

        private bool _active;

        public bool Active
        {
            get => _active;
            set
            {
                _active = value;
                foreach (var renderer in GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = _active;
                }
            }
        }
    }
}
