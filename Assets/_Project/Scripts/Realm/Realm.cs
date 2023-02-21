using System;
using Games.GrumpyBear.Core.Observables;
using UnityEngine;

namespace DualityGame.Realm
{
    [CreateAssetMenu(fileName = "Realm", menuName = "Duality/Realm", order = 0)]
    public class Realm : ScriptableObject
    {
        private static readonly Observable<Realm> _current = new();
        public static Realm Current => _current.Value;

        public static void Subscribe(Action<Realm> subscriber) => _current.Subscribe(subscriber);
        public static void Unsubscribe(Action<Realm> subscriber) => _current.Unsubscribe(subscriber);

        
        [SerializeField] private int _levelLayer;
        [SerializeField] private int _playerLayer;
        [field: SerializeField] public int LevelLayer { get; private set;  }
        [field: SerializeField] public int PlayerLayer { get; private set;  }
        [field: SerializeField] public Realm CanWarpTo { get; private set;  }

        public int LevelLayerMask => 1 << _levelLayer;
        public int PlayerLayerMask => 1 << _playerLayer;

        public int LayerMask => LevelLayerMask | PlayerLayerMask;
        
        public event Action OnEnter;
        public event Action OnLeave;

        public bool IsActive => _current.Value == this;
        
        public void SetActive()
        {
            if (_current.Value == this) return;
            if (_current.Value != null) _current.Value.OnLeave?.Invoke();
            _current.Set(this);
            _current.Value.OnEnter?.Invoke();
        }
    }
}
