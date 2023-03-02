using Games.GrumpyBear.Core.Observables.ScriptableObjects;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace DualityGame.Core
{
    [CreateAssetMenu(menuName = "Duality/Game State", fileName = "Game state", order = 0)]
    public class GameState : GlobalStateT<GameState>
    {
        [SerializeField] private bool _initialState = false;

        private void OnEnable()
        {
            #if UNITY_EDITOR
            if (!EditorApplication.isPlayingOrWillChangePlaymode) return;
            #endif
            if (!_initialState) return;
            Debug.Assert(Current == null, $"The can only be one initial state. Current initial state is {Current}", this);
            SetActive();
        }
    }
}
