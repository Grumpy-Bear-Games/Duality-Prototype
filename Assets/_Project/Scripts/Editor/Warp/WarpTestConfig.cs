using Unity.Properties;
using UnityEditor;
using UnityEngine;

namespace DualityGame.Editor.Warp
{
    [FilePath("Assets/WarpTestConfig.asset", FilePathAttribute.Location.ProjectFolder)]
    public class WarpTestConfig: ScriptableSingleton<WarpTestConfig>
    {
        [SerializeField] private float _radius;
        [SerializeField] private float _transition;
        [SerializeField] private DualityGame.Realm.Realm _currentRealm;
        [SerializeField] private DualityGame.Realm.Realm _warpToRealm;
        [SerializeField] private Vector3 _playerPosition;

        [CreateProperty]
        public float Radius
        {
            get => _radius;
            set
            {
                _radius = value;
                Save(true);
            }
        }

        [CreateProperty]
        public DualityGame.Realm.Realm CurrentRealm
        {
            get => _currentRealm;
            set
            {
                _currentRealm = value;
                Save(true);
            }
        }

        [CreateProperty]
        public DualityGame.Realm.Realm WarpToRealm
        {
            get => _warpToRealm;
            set
            {
                _warpToRealm = value;
                Save(true);
            }
        }

        [CreateProperty]
        public Vector3 PlayerPosition
        {
            get => _playerPosition;
            set
            {
                _playerPosition = value;
                Save(true);
            }
        }

        [CreateProperty]
        public float Transition
        {
            get => _transition;
            set
            {
                _transition = value;
                Save(true);
            }
        }
    }
}
