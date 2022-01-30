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
    }
}
