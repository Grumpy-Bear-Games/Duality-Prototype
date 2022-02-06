using UnityEngine;

namespace DualityGame.Realm
{
    [CreateAssetMenu(fileName = "Realm", menuName = "Duality/Realm", order = 0)]
    public class Realm : ScriptableObject
    {
        [SerializeField] private int _levelLayer;
        [SerializeField] private int _playerLayer;

        public int LevelLayer => _levelLayer;
        public int PlayerLayer => _playerLayer;

        public int LevelLayerMask => 1 << _levelLayer;
        public int PlayerLayerMask => 1 << _playerLayer;
    }
}
