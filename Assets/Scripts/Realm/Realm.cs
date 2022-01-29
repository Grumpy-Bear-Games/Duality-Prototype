using UnityEngine;

namespace DualityGame.Realm
{
    [CreateAssetMenu(fileName = "Realm", menuName = "Duality/Realm", order = 0)]
    public class Realm : ScriptableObject
    {
        [SerializeField] private LayerMask _levelLayers;
        [SerializeField] private int _playerLayer;

        public LayerMask LeveLayers => _levelLayers;
        public int PlayerLayer => _playerLayer;
    }
}
