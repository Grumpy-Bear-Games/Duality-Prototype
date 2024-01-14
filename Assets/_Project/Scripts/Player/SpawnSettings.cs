using DualityGame.Core;
using DualityGame.SaveSystem;
using DualityGame.VFX;
using UnityEngine;

namespace DualityGame.Player
{
    [CreateAssetMenu(menuName = "Duality/Portal Settings", fileName = "Portal Settings", order = 0)]
    public class SpawnSettings: ScriptableObject
    {
        [field: SerializeField] public GameSession GameSession { get; private set; }
        [field: SerializeField] public ScreenFader ScreenFader { get; private set; }
        [field: SerializeField] public GameState TransitionGameState { get; private set; }
    }
}
